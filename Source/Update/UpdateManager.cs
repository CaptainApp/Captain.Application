using System;
using static Captain.Application.Application;

#pragma warning disable 67

namespace Captain.Application {
  /// <inheritdoc />
  /// <summary>
  ///   Abstracts <see cref="T:Squirrel.UpdateManager" /> logic
  /// </summary>
  internal sealed class UpdateManager : IDisposable {
    /// <summary>
    ///   Determines whether or not the update manager is available
    /// </summary>
    internal UpdaterAvailability Availability { get; } = UpdaterAvailability.NotSupported;

    /// <summary>
    ///   Current update status
    /// </summary>
    internal UpdateStatus Status { get; } = UpdateStatus.Idle;

    /// <summary>
    ///   Initializes the update manager asynchronously
    /// </summary>
    internal UpdateManager() {
      if (!FsManager.IsFeatureAvailable) {
        Log.Warn("application directory unavailable - aborting");
      }

      // TODO: implement this
    }

    /// <inheritdoc />
    /// <summary>
    ///   Releases resources
    /// </summary>
    public void Dispose() {}

    /// <summary>
    ///   Triggered when the update manager availability changes
    /// </summary>
    /// <param name="manager">Update manager instance</param>
    /// <param name="availability">Updater status</param>
    internal delegate void AvailabilityChangedHandler(UpdateManager manager, UpdaterAvailability availability);

    /// <summary>
    ///   Triggered when the update status changes
    /// </summary>
    /// <param name="manager">Update manager instance</param>
    /// <param name="status">Status</param>
    internal delegate void UpdateStatusChangedHandler(UpdateManager manager, UpdateStatus status);

    /// <summary>
    ///   Triggered when the progress of the underlying update operation changes
    /// </summary>
    /// <param name="manager">Update manager instance</param>
    /// <param name="status">Status</param>
    /// <param name="progress">Operation progress</param>
    internal delegate void UpdateProgressChangedHandler(UpdateManager manager, UpdateStatus status, int progress);

    /// <summary>
    ///   Triggered when the update manager availability changes
    /// </summary>
    internal event AvailabilityChangedHandler OnAvailabilityChanged;

    /// <summary>
    ///   Triggered when the update manager status changes
    /// </summary>
    internal event UpdateStatusChangedHandler OnUpdateStatusChanged;

    /// <summary>
    ///   Triggered when the update manager progress changes
    /// </summary>
    internal event UpdateProgressChangedHandler OnUpdateProgressChanged;
  }
}