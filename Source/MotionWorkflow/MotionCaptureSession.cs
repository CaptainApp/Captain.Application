using System;
using System.Threading;
using Aperture;
using static Captain.Application.Application;

namespace Captain.Application {
  /// <inheritdoc />
  /// <summary>
  ///   Implements the basic logic behind motion workflows
  /// </summary>
  internal class MotionCaptureSession : IDisposable {
    /// <summary>
    ///   MediaFoundation codec instance
    /// </summary>
    private readonly VideoCodec codec;

    /// <summary>
    ///   Video capture device
    /// </summary>
    private readonly VideoCaptureDevice device;

    /// <summary>
    ///   Thread for performing the video encoding
    /// </summary>
    private readonly Thread encodingThread;

    /// <summary>
    ///   Gets the current workflow state
    /// </summary>
    private MotionCaptureState State { get; set; } = MotionCaptureState.Recording;

    /// <summary>
    ///   Gets whether this session has been disposed
    /// </summary>
    internal bool Disposed { get; private set; }

    /// <summary>
    ///   Creates a new motion capture session
    /// </summary>
    /// <param name="codec">The Media Foundation codec instance</param>
    /// <param name="device">The video provider instance</param>
    internal MotionCaptureSession(VideoCodec codec, VideoCaptureDevice device) {
      this.codec = codec;
      this.device = device;

      Log.Info("starting encoder thread");
      this.encodingThread = new Thread(ThreadStart) { Priority = ThreadPriority.BelowNormal };
      this.encodingThread.Start();
      Log.Debug($"encoder thread with ID {this.encodingThread.ManagedThreadId} has been started");
    }

    /// <summary>
    ///   Encoding thread entry point
    /// </summary>
    private void ThreadStart() {
      Log.Info("started encoder thread");
      this.codec.Start();

      while (true) {
        if (State == MotionCaptureState.Recording) {
          // capture a single frame
          this.device.AcquireFrame();

          // lock frame and obtain the time relative to the recording start
          VideoFrame frame = this.device.LockFrame();

          // encode the frame and release it
          this.codec.Feed(frame);
          this.device.UnlockFrame(frame);
        } else if (State == MotionCaptureState.Paused) {
          try {
            // wait for an infinite amount of time until the thread is awaken by an interrupt
            Thread.Sleep(Timeout.InfiniteTimeSpan);
          } catch (ThreadInterruptedException) {
            // TODO: update timing!
            Log.Trace("encoding thread interrupted");
          }
        } else {
          break;
        }
      }

      Log.Trace("returned from acquire-encode loop");
    }

    /// <inheritdoc />
    /// <summary>
    ///   Releases resources used by this session
    /// </summary>
    public void Dispose() {
      State = MotionCaptureState.Idle;

      Log.Debug("waiting for encoder thread to join");
      this.encodingThread?.Join();
      
      Disposed = true;
      GC.Collect();
    }
  }
}