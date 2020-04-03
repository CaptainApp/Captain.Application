namespace Captain.Application {
  /// <summary>
  ///   Different application update policies
  /// </summary>
  public enum UpdatePolicy {
    /// <summary>
    ///   Indicates that automatic updates are disabled
    /// </summary>
    Disabled = 0,

    /// <summary>
    ///   Checks for updates and asks the user whether to install them
    /// </summary>
    CheckOnly = 1,

    /// <summary>
    ///   Automatically downloads and installs updates
    /// </summary>
    Automatic = 2
  }
}