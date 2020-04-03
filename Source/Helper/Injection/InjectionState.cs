using System;

namespace Captain.Application {
  /// <summary>
  ///   Contains information about an injected library.
  /// </summary>
  internal struct InjectionState {
    /// <summary>
    ///   Open handle to the remote process.
    /// </summary>
    internal IntPtr RemoteProcessHandle;

    /// <summary>
    ///   Handle for the remotely loaded library.
    /// </summary>
    internal IntPtr RemoteModuleHandle;
  }
}