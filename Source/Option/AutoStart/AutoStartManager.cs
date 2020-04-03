using System;
using System.IO;
using System.Reflection;
using System.Security;
using Microsoft.Win32;
using static Captain.Application.Application;

namespace Captain.Application {
  /// <summary>
  ///   Handles the autostart feature by reading from/writing to the OS registry
  /// </summary>
  internal sealed class AutoStartManager {
    /// <summary>
    ///   Contains the Startup registry key path
    /// </summary>
    private const string StartupRegistryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

    /// <summary>
    ///   Contains the StartupApproved registry key path
    /// </summary>
    private const string ApprovedStartupRegistryKeyPath =
      @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run";

    /// <summary>
    ///   Current instnace of the StartupApproved registry key
    /// </summary>
    private RegistryKey approvedStartupRegistryKey;

    /// <summary>
    ///   Current instance of the Startup registry key
    /// </summary>
    private RegistryKey startupRegistryKey;

    /// <summary>
    ///   Whether or not this feature is available
    /// </summary>
    internal bool IsFeatureAvailable => this.startupRegistryKey != null;

    /// <summary>
    ///   Constructs an instance of this class
    /// </summary>
    internal AutoStartManager() {
      OpenStartupKey();
    }

    /// <summary>
    ///   Opens the Startup registry key
    /// </summary>
    /// <returns>Whether or not the operation completed successfully</returns>
    private void OpenStartupKey() {
      try {
        Log.Trace("opening generic startup key");
        this.startupRegistryKey = Registry.CurrentUser.OpenSubKey(StartupRegistryKeyPath,
          RegistryKeyPermissionCheck.ReadWriteSubTree);

        Log.Trace("opening approved startup key");
        this.approvedStartupRegistryKey = Registry.CurrentUser.OpenSubKey(ApprovedStartupRegistryKeyPath,
          RegistryKeyPermissionCheck.ReadWriteSubTree);
      } catch (SecurityException) {
        Log.Warn("access is denied to the registry key - some features may be unavailable");
      }
    }

    /// <summary>
    ///   Determines whether the application is set to run at login
    /// </summary>
    /// <returns>The current auto-start policy</returns>
    internal AutoStartPolicy GetAutoStartPolicy() {
      // make sure there's a valid path in the generic registry key and that matches exactly with the current
      // executable path
      try {
        if (!String.Equals(Path.GetFullPath(this.startupRegistryKey.GetValue(
              System.Windows.Forms.Application.ProductName,
              null)
            .ToString()),
          Path.GetFullPath(Assembly.GetExecutingAssembly().Location),
          StringComparison.InvariantCultureIgnoreCase)) { return AutoStartPolicy.Disapproved; }
      } catch (NullReferenceException) {
        // startup path is null (no auto-start entry found)
        return AutoStartPolicy.Disapproved;
      }

      try {
        if (this.approvedStartupRegistryKey != null &&
            this.approvedStartupRegistryKey.GetValue(System.Windows.Forms.Application.ProductName) is byte[] data) {
          return (AutoStartPolicy) BitConverter.ToInt32(data, 0);
        }
      } catch (Exception exception) when (exception is SecurityException ||
                                          exception is IOException ||
                                          exception is UnauthorizedAccessException) {
        Log.Warn($"could not open approved startup registry key - ${exception}");
      }
      // startup approval is likely to be unsupported on this platform
      return AutoStartPolicy.Approved;
    }

    /// <summary>
    ///   Creates or deletes the value on the Startup registry key
    /// </summary>
    /// <param name="policy">Explicitly set the policy</param>
    /// <param name="hard">If true, all entries will be recreated/deleted</param>
    /// <returns>The policy after completing the operation</returns>
    internal AutoStartPolicy ToggleAutoStart(AutoStartPolicy? policy = null, bool hard = false) {
      try {
        if (this.approvedStartupRegistryKey != null &&
            this.approvedStartupRegistryKey.GetValue(System.Windows.Forms.Application.ProductName) is byte[] data) {
          // has approved startup key - retrieve the policy to be set
          policy = policy ??
                   ((AutoStartPolicy) BitConverter.ToInt32(data, 0) == AutoStartPolicy.Approved // invert current policy
                     ? AutoStartPolicy.Disapproved
                     : AutoStartPolicy.Approved);

          // update auto-start policy
          if (hard && policy == AutoStartPolicy.Disapproved) {
            Log.Warn("[hard mode] deleting approved startup value");
            this.approvedStartupRegistryKey.DeleteValue(System.Windows.Forms.Application.ProductName);
          } else if (!hard) {
            Log.Trace($"updating automatic startup policy: {policy}");
            this.approvedStartupRegistryKey.SetValue(System.Windows.Forms.Application.ProductName,
              BitConverter.GetBytes((int) policy));
            return policy.Value;
          }
        }

        if (!policy.HasValue) {
          policy = GetAutoStartPolicy() == AutoStartPolicy.Approved
            ? AutoStartPolicy.Disapproved
            : AutoStartPolicy.Approved;
        }

        // delete/set the registry entry
        if (policy == AutoStartPolicy.Approved) {
          Log.Trace("setting auto-start value in generic registry key");
          this.startupRegistryKey.SetValue(System.Windows.Forms.Application.ProductName,
            Assembly.GetExecutingAssembly().Location,
            RegistryValueKind.String);

          // create entry in the approved startup key if applicable
          this.approvedStartupRegistryKey?.SetValue(System.Windows.Forms.Application.ProductName,
            BitConverter.GetBytes((int) AutoStartPolicy.Approved));
        } else {
          Log.Trace("deleting auto-start value in generic registry key");
          this.startupRegistryKey.DeleteValue(System.Windows.Forms.Application.ProductName, false);
        }

        return policy.Value;
      } catch (SecurityException) {
        Log.Warn("access to the registry key is denied!");
        return policy.GetValueOrDefault(AutoStartPolicy.Approved) == AutoStartPolicy.Approved
          ? AutoStartPolicy.Disapproved
          : AutoStartPolicy.Approved;
      }
    }
  }
}