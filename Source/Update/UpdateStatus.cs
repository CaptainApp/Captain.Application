namespace Captain.Application {
  internal enum UpdateStatus {
    /// <summary>
    ///   Update manager is idle
    /// </summary>
    Idle = 0,

    /// <summary>
    ///   The application is checking for updates online
    /// </summary>
    CheckingForUpdates = 1,

    /// <summary>
    ///   Updates have been found and are ready to be downloaded
    /// </summary>
    DownloadingUpdates = 2,

    /// <summary>
    ///   Updates have been downloaded and are ready to be installed
    /// </summary>
    ApplyingUpdates = 3,

    /// <summary>
    ///   Updates have been applied and the application is ready to restart
    /// </summary>
    ReadyToRestart = 4
  }
}