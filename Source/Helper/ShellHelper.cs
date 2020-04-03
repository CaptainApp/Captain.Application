using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Captain.Common.Native;
using static Captain.Application.Application;

namespace Captain.Application {
  internal static class ShellHelper {
    /// <summary>
    ///   Reveals a file in a File Explorer window.
    ///   TODO: remove Thread bullshit and use proper async code
    /// </summary>
    /// <param name="path">Path of the file to be revealed.</param>
    internal static async void RevealInExplorerAsync(string path) {
      var thread = new Thread(() => {
        string folderPath = Path.GetDirectoryName(path);
        int hr;

        // parse containing folder and file path
        if ((hr = Shell32.SHParseDisplayName(folderPath, IntPtr.Zero, out IntPtr nativeFolder, 0, out _)) != 0 ||
            nativeFolder == default ||
            (hr = Shell32.SHParseDisplayName(path, IntPtr.Zero, out IntPtr nativeFile, 0, out _)) != 0 ||
            nativeFile == default) {
          // shell invoke failed or a null pointer was yielded for native shell items
          Log.Warn($"SHParseDisplayName() failed: HRESULT 0x{hr:x8}");

          // release used resources
          if (nativeFolder != default) { Marshal.FreeCoTaskMem(nativeFolder); }

          return;
        }

        // open the file explorer window
        if ((hr = Shell32.SHOpenFolderAndSelectItems(nativeFolder, 1, new[] { nativeFile }, 0)) != 0) {
          Log.Warn($"SHOpenFolderAndSelectItems() failed: HRESULT 0x{hr:x8}");
        }

        // release resources
        Marshal.FreeCoTaskMem(nativeFolder);
        Marshal.FreeCoTaskMem(nativeFile);
      });

      thread.TrySetApartmentState(ApartmentState.MTA);
      thread.Start();

      await new Task(() => thread.Join());
    }
  }
}