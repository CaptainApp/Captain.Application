namespace Captain.Application {
  /// <summary>
  ///   Represents an icon kind
  /// </summary>
  internal enum IndicatorStatus {
    /// <summary>
    ///   Represents the application icon
    /// </summary>
    Idle,

    /// <summary>
    ///   Represents the application icon with a red dot indicating the capture mode
    /// </summary>
    Recording,

    /// <summary>
    ///   Represents the application icon with a warning badge
    /// </summary>
    Warning,

    /// <summary>
    ///   Represents the application icon with a success badge
    /// </summary>
    Success,

    /// <summary>
    ///   Represents an indeterminate progress indicator
    /// </summary>
    Progress
  }
}