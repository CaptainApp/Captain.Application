using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Aperture;
using Captain.Common;
using Microsoft.Win32;

#if DEBUG
using SharpDX;
using SharpDX.Diagnostics;
#endif

namespace Captain.Application {
  /// <summary>
  ///   Defines application entry logic
  /// </summary>
  internal static class Application {
    #region Private fields and constants

    /// <summary>
    ///   Current logger file stream
    /// </summary>
    private static Stream loggerStream;

    /// <summary>
    ///   Single instance mutex name
    /// </summary>
    private static string SingleInstanceMutexName =>
      $"{System.Windows.Forms.Application.ProductName} Instance Mutex ({Guid})";

    #endregion

    #region App execution control methods

    /// <summary>
    ///   Terminates the program gracefully
    /// </summary>
    /// <param name="exitCode">Optional exit code</param>
    /// <param name="exit">Whether to actually exit or just perform cleanup tasks</param>
    internal static void Exit(int exitCode = 0, bool exit = true) {
      try {
        Log.Info($"exiting with code 0x{exitCode:x8}");

        DesktopKeyboardHook?.Dispose();
        DesktopMouseHook?.Dispose();
        TrayIcon?.Hide();
        Options?.Save();
        HudManager?.Dispose();
        UpdateManager?.Dispose();

#if DEBUG
        Log.Trace("reporting unmanaged SharpDX objects:");
        Log.Trace(ObjectTracker.ReportActiveObjects());
#endif

        loggerStream.Dispose();
        Log.Streams.Clear();

        GC.WaitForPendingFinalizers();
      } catch (Exception exception) {
        // try to log exceptions
        if (Log.Streams?.Count > 0 && Log.Streams.All(s => s.CanWrite)) {
          Log.Error("exception caught during exit: " + exception);
        }
      } finally {
        if (exit) {
          Environment.ExitCode = exitCode;
          System.Windows.Forms.Application.Exit();
        }
      }
    }

    /// <summary>
    ///   Restarts the application
    /// </summary>
    /// <param name="exitCode">Optional exit code</param>
    internal static void Restart(int exitCode = 0) {
      Log.Info("restarting");

      try {
        Exit(exitCode, false);
        Process.Start(Assembly.GetExecutingAssembly().Location, $"--kill {Process.GetCurrentProcess().Id}");
      } finally {
        Environment.ExitCode = exitCode;
        System.Windows.Forms.Application.Exit();
      }
    }

    /// <summary>
    ///   Resets the application options
    /// </summary>
    /// <param name="hard">Removes everything under the application directory</param>
    internal static void Reset(bool hard = false) {
      var nodes = new List<string> {Path.Combine(FsManager.GetSafePath(), Options.OptionsFileName)};

      if (hard) {
        Log.Warn("performing hard reset");
        nodes.AddRange(new[] {FsManager.LogsPath, FsManager.PluginPath, FsManager.TemporaryPath}
                         .Select(FsManager.GetSafePath));
      }

      Log.Warn("deleting nodes: " + String.Join(", ", nodes));
      Exit(0, false);

      Process.Start(Assembly.GetExecutingAssembly().Location,
                    $"--kill {Process.GetCurrentProcess().Id} --rmnodes \"{String.Join("\" \"", nodes)}\"");
      Environment.Exit(0);
    }

    #endregion

    #region App globals

    /// <summary>
    ///   Single instance mutex
    /// </summary>
    private static Mutex SingleInstanceMutex { get; set; }

    /// <summary>
    ///   Assembly GUID
    /// </summary>
    private static string Guid =>
      Assembly.GetExecutingAssembly().GetCustomAttribute<GuidAttribute>().Value;

    /// <summary>
    ///   Assembly product version
    /// </summary>
    internal static Version Version => Assembly.GetExecutingAssembly().GetName().Version;

    #endregion

    #region App components

    /// <summary>
    ///   Application-wide logger
    /// </summary>
    internal static Logger Log { get; private set; }

    /// <summary>
    ///   Handles local application filesystem
    /// </summary>
    internal static FsManager FsManager { get; private set; }

    /// <summary>
    ///   Handles tray icon
    /// </summary>
    internal static TrayIcon TrayIcon { get; private set; }

    /// <summary>
    ///   Application <see cref="Options" /> instance
    /// </summary>
    internal static Options Options { get; private set; }

    /// <summary>
    ///   Application update manager
    /// </summary>
    internal static UpdateManager UpdateManager { get; private set; }

    /// <summary>
    ///   Keyboard hook provider for system UI
    /// </summary>
    internal static DesktopKeyboardHook DesktopKeyboardHook { get; private set; }

    /// <summary>
    ///   Mouse hook provider for system UI
    /// </summary>
    internal static DesktopMouseHook DesktopMouseHook { get; private set; }

    /// <summary>
    ///   Handles heads-up displays
    /// </summary>
    internal static HudManager HudManager { get; private set; }

    /// <summary>
    ///   Handles extensions
    /// </summary>
    internal static ExtensionManager ExtensionManager { get; private set; }

    #endregion

#if DEBUG
    /// <summary>
    ///   Reports active SharpDX objects
    /// </summary>
    internal static void ReportObjects() {
      List<ObjectReference> objects = ObjectTracker.FindActiveObjects();

      if (!objects.Any()) {
        Log.Info("no active objects to be reported!");
        return;
      }

      Log.Warn($"*** there are {objects.Count} active object(s) ***");

      foreach (ObjectReference obj in objects) {
        Log.Warn($"active {obj.Object.Target.GetType()} ({(obj.IsAlive ? "alive" : "dead")})");

        foreach (string line in obj.StackTrace.Split('\n').Where(l => l.Contains(".cs:"))) {
          Log.Warn(line);
        }
      }
    }
#endif

    /// <summary>
    ///   Handles command line arguments
    ///   TODO: is this really necessary?
    /// </summary>
    /// <param name="args">CLI arguments</param>
    private static void HandleCommandLineArgs(string[] args) {
      // ReSharper disable PatternAlwaysMatches
      if (Array.IndexOf(args, "--rmnodes") is int i && i != -1) {
        for (int j = i + 1; j < args.Length; j++) {
          try {
            FileAttributes attributes = File.GetAttributes(args[j]);

            if ((attributes & FileAttributes.Directory) != 0) {
              try {
                Directory.Delete(args[j], true);
              } catch {
                Console.WriteLine(@"failed to delete directory - {0}", args[j]);
              }
            } else {
              try {
                File.Delete(args[j]);
              } catch {
                Console.WriteLine(@"failed to delete file - {0}", args[j]);
              }
            }
          } catch {
            Console.WriteLine(@"failed to retrieve file attributes - {0}", args[j]);
          }
        }
      }

      // ReSharper disable once PatternAlwaysMatches
      if (Array.IndexOf(args, "--kill") is int k &&
          k != -1 &&
          1 + k < args.Length &&
          UInt16.TryParse(args[1 + k], out ushort pid)) {
        try {
          Process.GetProcessById(pid).Kill();
        } catch {
          Console.WriteLine(@"failed to kill process with ID {0} (already dead?)", pid);
        }
      }
    }

    /// <summary>
    ///   Performs initial setup tasks, such as displaying the Welcome dialog if pertinent or install application
    ///   shortcuts
    /// </summary>
    private static void InitialSetup() {
      // has the application been updated or perhaps is it the first time the user opens it?
      if (Options.LastVersion != Version.ToString()) {
        Options.LastVersion = Version.ToString();
      }
    }

    /// <summary>
    ///   Let Windows know we're supposed to be run from an integrated graphics adapter on hybrid systems
    /// </summary>
    private static void EnforceIntegratedGraphics() {
      using (RegistryKey key =
        Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\DirectX\UserGpuPreferences", true)) {

        if (key != null) {
          // if supported, use Windows 10 graphics performance settings
          string name = Assembly.GetEntryAssembly().Location;
          string currentValue = (key.GetValue(name, "") as string ?? "").ToLower();
          if (!currentValue.Contains("gpupreference=1")) {
            Log.Warn("integrated graphics not enforcing - updating graphics preferences");
            key.SetValue(name, "GpuPreference=1;");
            Restart();
          } 
        }
      }
    }

    /// <summary>
    ///   Program entry point
    /// </summary>
    /// <param name="args">Command-line arguments passed to the program</param>
    [STAThread]
    private static void Main(string[] args) {
#if DEBUG
      // black magic - tracks unmanaged D2D/D3D objects and prints out unreleased resources at exit
      Configuration.EnableObjectTracking = true;
      ObjectTracker.StackTraceProvider = () => Environment.StackTrace;
#endif

      HandleCommandLineArgs(args);

      // is another instance of the application currently running?
      // TODO: allow multiple instances with a key switch (i.e. Shift)
      if (Mutex.TryOpenExisting(SingleInstanceMutexName, out Mutex _)) {
        Environment.Exit(1);
      }

      // create a mutex that will prevent multiple instances of the application to be run
      SingleInstanceMutex = new Mutex(true, SingleInstanceMutexName);

      // Windows Forms setup
      System.Windows.Forms.Application.EnableVisualStyles();
      System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

#if !DEBUG
      System.Windows.Forms.Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException, true);
      System.Windows.Forms.Application.ThreadException
 += (s, e) => {
        Log.Error("unhandled exception: ${e.Exception}");
        Restart(e.Exception.HResult);
      };
#endif

      // initialize file system manager and log file stream
      Log = new Logger();
      FsManager = new FsManager();

      try {
        // create/open log file stream
        loggerStream = new FileStream(Path.Combine(FsManager.GetSafePath(FsManager.LogsPath),
                                                   DateTime.UtcNow.ToString("yy.MM.dd") + ".log"),
                                      FileMode.Append);
        Log.Streams.Add(loggerStream);
      } catch (Exception exception) {
        Log.Warn($"could not open logger stream: {exception}");
      }

      // write version info to log
      Log.Info(
        $"{System.Windows.Forms.Application.ProductName} {Version} ({(Environment.Is64BitProcess ? 64 : 32)}-bit)");
      Log.Info($"{Environment.OSVersion} ({(Environment.Is64BitOperatingSystem ? 64 : 32)}-bit)");

      // initialize main components
      Options = Options.Load() ?? new Options();
      ExtensionManager = new ExtensionManager();

      EnforceIntegratedGraphics();
      InitialSetup();

      TrayIcon = new TrayIcon();
      UpdateManager = new UpdateManager();

      // global hook behaviours
      DesktopKeyboardHook = new DesktopKeyboardHook();
      DesktopMouseHook = new DesktopMouseHook();

      // HUD manager depends on the hooks
      HudManager = new HudManager();
      DesktopKeyboardHook.RequestLock();

      // release the mutex when the application is terminated
      System.Windows.Forms.Application.ApplicationExit += (s, e) => {
        lock (SingleInstanceMutex) {
          SingleInstanceMutex.ReleaseMutex();
        }
      };

      new Workflow {
        Codec = (typeof(HevcMediaFoundationVideoCodec).FullName, null),
        Handlers = new (string, object)[] {(typeof(FileHandler).FullName, null)},
        Name = "test",
        Type = WorkflowType.Motion,
        Region = new Region {
          Type = RegionType.Manual
        }
      }.StartAsync(HudManager.GetContainer());

      //new WorkflowPropertyDialog().Show();

      // start application event loop
      System.Windows.Forms.Application.Run();
    }
  }
}