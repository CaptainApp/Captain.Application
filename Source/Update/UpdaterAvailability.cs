namespace Captain.Application {
  /// <summary>
  ///   Represents updater module availability states
  /// </summary>
  internal enum UpdaterAvailability {
    /// <summary>
    ///   Application updates are not supported (i.e.: missing libraries, app not installed)
    /// </summary>
    NotSupported,

    /// <summary>
    ///   Updater is temporarily unavailable
    /// </summary>
    NotAvailable,

    /// <summary>
    ///   Updater is available
    /// </summary>
    FullyAvailable
  }
}