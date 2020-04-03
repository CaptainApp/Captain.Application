using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Captain.Common.Native;
using static Captain.Application.Application;

namespace Captain.Application {
  /// <summary>
  ///   Helper methods for injecting code and manipulating remote processes.
  /// </summary>
  internal static class InjectionHelper {
    /// <summary>
    ///   Injects a library to the target process.
    /// </summary>
    /// <remarks>
    ///   TODO: perform injection in a secondary thread so we don't block the application message loop
    /// </remarks>
    /// <param name="pid">Process ID</param>
    /// <param name="path32">32-bit library path</param>
    /// <param name="path64">64-bit library path</param>
    /// <returns>An struct containing the injection state.</returns>
    internal static InjectionState Inject(int pid, string path32, string path64) {
      // TODO: only set actually needed permissions and retry with more extensive ones if access gets denied
      //       in one of the following calls
      const Kernel32.ProcessAccessRights accessRights = Kernel32.ProcessAccessRights.PROCESS_CREATE_THREAD |
                                                        Kernel32.ProcessAccessRights.PROCESS_QUERY_INFORMATION |
                                                        Kernel32.ProcessAccessRights.PROCESS_VM_OPERATION |
                                                        Kernel32.ProcessAccessRights.PROCESS_VM_WRITE |
                                                        Kernel32.ProcessAccessRights.PROCESS_VM_READ;

      // create injection state struct
      var state = new InjectionState();

      // open process handle
      IntPtr processHandle = Kernel32.OpenProcess(accessRights, false, pid);
      if (processHandle == IntPtr.Zero) {
        throw new Win32Exception(Marshal.GetLastWin32Error());
      }

      state.RemoteProcessHandle = processHandle;
      Log.Debug($"handle for process {pid} opened successfully");

      // try to guess if this is a 32- or 64-bit process
      if (!Kernel32.IsWow64Process(processHandle, out bool isWowProcess)) {
        Log.Warn("could not guess remote process architecture - trying with 32-bit library");
      }

      // get path for the target process architecture
      string path = isWowProcess ? path32 : Environment.Is64BitOperatingSystem ? path64 : path32;
      int pathBufferLength = 2 * path.Length + 1; // LoadLibraryW is Unicode
      Log.Debug($"library to be injected: {path}");

      // allocate buffer for library path, in the remote virtual address space
      IntPtr remotePathBuffer = Kernel32.VirtualAllocEx(processHandle,
                                                        IntPtr.Zero,
                                                        pathBufferLength,
                                                        Kernel32.AllocationType.MEM_COMMIT,
                                                        Kernel32.MemoryProtectionFlags.PAGE_READWRITE);
      if (remotePathBuffer == IntPtr.Zero) {
        int lastError = Marshal.GetLastWin32Error();
        Kernel32.CloseHandle(processHandle);
        throw new Win32Exception(lastError);
      }

      Log.Debug($"allocated {pathBufferLength} bytes in remote process memory (0x{remotePathBuffer.ToInt64():x8})");

      // allocate local string buffer and write to the remote process memory
      IntPtr localPathBuffer = Marshal.StringToHGlobalUni(path);
      if (!Kernel32.WriteProcessMemory(processHandle, remotePathBuffer, localPathBuffer, pathBufferLength, out _)) {
        int lastError = Marshal.GetLastWin32Error();
        Kernel32.VirtualFreeEx(processHandle, remotePathBuffer, pathBufferLength, Kernel32.FreeType.MEM_DECOMMIT);
        Marshal.FreeHGlobal(localPathBuffer);
        Kernel32.CloseHandle(processHandle);
        throw new Win32Exception(lastError);
      }

      Log.Debug($"wrote {pathBufferLength} bytes to remote process memory");
      Marshal.FreeHGlobal(localPathBuffer);

      // get the LoadLibraryW function address
      IntPtr kernelModule = Kernel32.LoadLibrary(nameof(Kernel32));
      IntPtr loadLibraryPtr = Kernel32.GetProcAddress(kernelModule, "LoadLibraryW");
      Kernel32.FreeLibrary(kernelModule);

      // create remote thread for loading the library
      IntPtr threadHandle = Kernel32.CreateRemoteThread(processHandle,
                                                        IntPtr.Zero,
                                                        0,
                                                        loadLibraryPtr,
                                                        remotePathBuffer,
                                                        0,
                                                        out int threadId);

      if (threadHandle == IntPtr.Zero) {
        int lastError = Marshal.GetLastWin32Error();
        Kernel32.VirtualFreeEx(processHandle, remotePathBuffer, pathBufferLength, Kernel32.FreeType.MEM_DECOMMIT);
        Kernel32.CloseHandle(processHandle);
        throw new Win32Exception(lastError);
      }

      Log.Debug($"waiting for library load thread with ID {threadId} to terminate");
      Kernel32.WaitForSingleObject(threadHandle, unchecked((int) Kernel32.Infinite));
      Log.Debug("library load thread terminated - retrieving module handle");

      // on 64-bit platforms, handles do not fit in a DWORD value, so the module handle gets truncated. We'll use some
      // Toolhelp utilities to retrieve the list of loaded modules so we can release our library later in Eject()
      // TODO: debug this further?
      try {
        state.RemoteModuleHandle = Process.GetProcessById(pid)
                                          .Modules.Cast<ProcessModule>()
                                          .First(m => m.FileName == path)
                                          .BaseAddress;
      } catch {
        Log.Warn("could not retrieve remote module handle!");
      }

      Log.Debug("relasing resources");
      Kernel32.CloseHandle(threadHandle);
      Kernel32.VirtualFreeEx(processHandle, remotePathBuffer, pathBufferLength, Kernel32.FreeType.MEM_DECOMMIT);
      return state;
    }

    /// <summary>
    ///   Releases remote resources used by a previously injected module.
    /// </summary>
    /// <param name="state">Injection state struct.</param>
    internal static void Eject(InjectionState state) {
      // get the FreeLibrary function address
      IntPtr kernelModule = Kernel32.LoadLibrary(nameof(Kernel32));
      IntPtr freeLibraryPtr = Kernel32.GetProcAddress(kernelModule, "FreeLibrary");
      Kernel32.FreeLibrary(kernelModule);

      // create remote thread for loading the library
      Log.Debug("ejecting remote library");
      IntPtr threadHandle = Kernel32.CreateRemoteThread(state.RemoteProcessHandle,
                                                        IntPtr.Zero,
                                                        0,
                                                        freeLibraryPtr,
                                                        state.RemoteModuleHandle,
                                                        0,
                                                        out _);

      if (threadHandle == IntPtr.Zero) {
        int lastError = Marshal.GetLastWin32Error();
        Kernel32.CloseHandle(state.RemoteProcessHandle);
        throw new Win32Exception(lastError);
      }

      if (Kernel32.GetExitCodeThread(threadHandle, out int exitCode)) {
        if (exitCode == 1) {
          Log.Info("library ejection succeeded!");
        } else {
          Log.Warn("could not eject remote library!");
        }
      } else {
        Log.Warn("library ejection status unknown");
      }

      Log.Debug("closing remote process handle");
      Kernel32.CloseHandle(state.RemoteProcessHandle);
    }
  }
}