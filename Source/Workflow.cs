using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aperture;
using Captain.Common;
using Captain.UI;
using static Captain.Application.Application;
using Device = SharpDX.DXGI.Device;

namespace Captain.Application {
  /// <inheritdoc cref="WorkflowBase" />
  /// <summary>
  ///   Workflows are the functional unit of the applicatiod and provide the user a great extent of flexibility when
  ///   performing and automating capture tasks
  /// </summary>
  /// <remarks>
  ///   <see cref="T:Captain.Application.Workflow" /> is an internal type which implements the application logic for working
  ///   with workflows
  /// </remarks>
  internal sealed class Workflow : WorkflowBase {
    /// <summary>
    ///   HUD clipper component
    /// </summary>
    private Clipper clipper;

    /// <summary>
    ///   Media codec instance
    /// </summary>
    private Codec codec;

    /// <summary>
    ///   Stream handlers
    /// </summary>
    private Handler[] handlers;

    /// <summary>
    ///   Destination stream
    /// </summary>
    private MultiStream stream;

    /// <summary>
    ///   Current video capture device
    /// </summary>
    private VideoCaptureDevice captureDevice;

    /// <summary>
    ///   Recording toolbar
    /// </summary>
    private Toolbar toolbar;

    /// <summary>
    ///   Current container
    /// </summary>
    private HudContainerInfo currentContainer;

    /// <summary>
    ///   Capture session for motion workflows
    /// </summary>
    private MotionCaptureSession motionSession;

    /// <summary>
    ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources
    /// </summary>
    private void Dispose() {
      this.clipper?.Dispose();
      this.clipper = null;

      this.toolbar?.Dispose();
      this.toolbar = null;

      this.motionSession?.Dispose();
      this.motionSession = null;

      this.codec?.Dispose();
      this.codec = null;

      this.captureDevice?.Dispose();
      this.captureDevice = null;

      this.stream?.Dispose();
      this.stream = null;

      if (this.handlers != null) {
        foreach (Handler handler in this.handlers) {
          handler.Dispose();
        }
      }

      this.handlers = null;
    }

    /// <summary>
    ///   Sets the region for this workflow
    /// </summary>
    private async Task AcquireRegionAsync() {
      switch (Region.Type) {
        case RegionType.Manual: // user-selected region
          // create and unlock clipper component synchronously
          // TODO: review resource usage and cliper object lifetime
          this.clipper = new Clipper(this.currentContainer);
          await this.clipper.UnlockAsync();

          // user selected an area
          Region.Location = (this.clipper.Area.X, this.clipper.Area.Y);
          Region.Size = (this.clipper.Area.Width, this.clipper.Area.Height);
          break;

        case RegionType.ActiveMonitor: {
          // current monitor
          Rectangle bounds = Screen.GetBounds(Control.MousePosition);
          Region.Location = (bounds.X, bounds.Y);
          Region.Size = (bounds.Width, bounds.Height);
          break;
        }

        case RegionType.ActiveWindow: // active window
          // TODO: implement this -- P.S. would GetForegroundWindow() be enough?
          throw new NotImplementedException();

        case RegionType.Fixed: // fixed region
          break;

        case RegionType.FullDesktop: // entire desktop
          Region.Location = (SystemInformation.VirtualScreen.X, SystemInformation.VirtualScreen.Y);
          Region.Size = (SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
          break;
      }

      Log.Debug($"acquired region: {Region.Location}, {Region.Size}");
    }

    /// <summary>
    ///   Starts the workflow interactively
    /// </summary>
    internal async Task StartAsync(HudContainerInfo container) {
      this.currentContainer = container;

      Log.Info($"starting {Type} workflow on {container.ContainerType} container");
      await AcquireRegionAsync();

      if (Region.Size.Width <= Clipper.MinimumWidth || Region.Size.Height <= Clipper.MinimumHeight) {
        Log.Trace("capture dismissed");
        Finish();
        return;
      }

      if (this.captureDevice == null || !Equals(this.captureDevice.Size.ToTuple(), Region.Size.ToTuple())) {
        // dispose old video provider if size changed
        this.captureDevice?.Dispose();

        // video provider will capture frames
        Log.Debug("creating capture device");
        this.captureDevice = CaptureDeviceFactory.CreateVideoCaptureDevice(
          Region.Location.X,
          Region.Location.Y,
          Region.Size.Width,
          Region.Size.Height);

        // TODO: create and bind transforms
      }

      // create destination stream, a MultiStream which will act as some kind of stream multiplexer wrapping multiple
      // "sub-streams" onto one
      // TODO: add IImmediateHandler's to the MultiStream
      this.stream = new MultiStream();

      // create codec instance
      if (Type == WorkflowType.Still) {
        this.codec = Application.ExtensionManager.CreateObject<StillImageCodec>(Codec.TypeName,
                                                                                Region.Size.Width,
                                                                                Region.Size.Height,
                                                                                this.stream);
      } else if (Type == WorkflowType.Motion) {
        this.codec = Application.ExtensionManager.CreateObject<VideoCodec>(Codec.TypeName,
                                                                           Region.Size.Width,
                                                                           Region.Size.Height,
                                                                           this.stream);
      }

      // create handlers
      this.handlers = Handlers.Select(handlerData => {
        Handler handler = Application.ExtensionManager.CreateObject<Handler>(
          handlerData.TypeName,
          this,
          this.codec,
          this.stream,
          handlerData.Options);

        if (handler is IStreamWrapper streamWrapperHandler) {
          // add stream wrapper to the multi-stream
          this.stream.Add(streamWrapperHandler.OutputStream);
          Log.Trace("added stream wrapper handler: " + handlerData.TypeName);
        }

        return handler;
      }).ToArray();

      if (Type == WorkflowType.Still) {
        // still image -- we do not need our stream to be filesystem-backed, as still images can't really be *that*
        // large... I hope
        this.stream.Add(new MemoryStream());

        Log.Info("capturing screen");
        this.captureDevice.AcquireFrame();
        VideoFrame frame = this.captureDevice.LockFrame();

        Log.Info("encoding capture");
        this.codec.Start();
        this.codec.Feed(frame);
        this.codec.Dispose();

        // unlock and release capture
        this.captureDevice.UnlockFrame(frame);
        this.captureDevice.ReleaseFrame();

        Finish();
      } else {
        // motion capture -- stand back and don't underestimate my expertise on googling
        // save output to a temporary file
        Log.Info("starting recording session");

        // create toolbar
        this.toolbar = new Toolbar(Application.HudManager.GetContainer());
        this.clipper?.AttachToolbar(this.toolbar);

        /* bind toolbar events */
        // option events
        this.toolbar.OnOptionsRequested += delegate(object sender, ToolbarOptionRequestType optionType) {
          switch (optionType) {
            case ToolbarOptionRequestType.Generic:
              try {
                new OptionsWindow().Show();
              } catch {
                /* already open */
              }

              break;

            default:
              throw new NotImplementedException();
          }
        };

        // recording control intents
        this.toolbar.OnRecordingIntentReceived += delegate(object sender, ToolbarRecordingControlIntent controlIntent) {
          try {
            Log.Info($"received recording control intent: {controlIntent}");
            this.toolbar.SetPrimaryButtonState(enabled: false);

            switch (controlIntent) {
              case ToolbarRecordingControlIntent.Start:
                /* start recording */
                if (!(this.motionSession?.Disposed ?? true)) {
                  throw new InvalidOperationException("A previous motion capture session has not been closed");
                }

                if (!(this.codec is VideoCodec)) {
                  throw new NotSupportedException("The current codec does not allow multiple frames");
                }

                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (
                  this.captureDevice is DxgiVideoCaptureDevice
                    dxgiCaptureDevice && // make sure our capture device is DXGI-enabled
                  this.codec is IDxgiEnabledVideoCodec dxgiCodec) {
                  // make sure our codec is DXGI-compatible
                  // make sure we have the same D3D device for all sources
                  Device device = dxgiCaptureDevice.Devices.First();
                  if (dxgiCaptureDevice.Devices.All(d => d == device)) {
                    // set device to be bound to the DXGI device manager
                    // TODO: optimize DxgiVideoProvider so that it only creates one device for each adapter
                    // TODO: allow ID3DCodec's to operate on multiple DXGI devices
                    dxgiCodec.BindDevice(device);
                    Log.Info("bound DXGI device to codec");
                  }
                }

                // create motion capture session and change toolbar button intent
                this.motionSession = new MotionCaptureSession((VideoCodec) this.codec, this.captureDevice);
                this.toolbar.SetPrimaryButtonState(ToolbarRecordingControlIntent.Stop);
                break;

              case ToolbarRecordingControlIntent.Stop:
                this.motionSession.Dispose();
                Finish();
                break;

              default:
                throw new NotImplementedException();
            }
          } finally {
            this.toolbar?.SetPrimaryButtonState(enabled: true);
          }
        };
      }
    }

    /// <summary>
    ///   Common final code for handling the capture
    /// </summary>
    private void Finish() {
      // finalize and dispose encoder
      Log.Info("finalizing capture");
      this.codec?.Dispose();
      this.codec = null;

      if (this.handlers != null) {
        // handle capture
        Log.Info("triggering handlers");

        foreach (Handler handler in this.handlers) {
          handler.Handle();
        }
      }

      // dispose the rest of resources
      Dispose();
      Log.Info("workflow is done");
    }
  }
}