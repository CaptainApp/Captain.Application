namespace Captain.Application {
  /// <summary>
  ///   Enumerates the possible motion capture states
  /// </summary>
  internal enum MotionCaptureState {
    /// <summary>
    ///   A motion capture session has not yet been started
    /// </summary>
    Idle,

    /// <summary>
    ///   A motion capture session is active and ongoing
    /// </summary>
    Recording,

    /// <summary>
    ///   A motion capture session has been started but is currently paused
    /// </summary>
    Paused
  }
}
