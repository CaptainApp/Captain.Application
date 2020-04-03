using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Aperture;
using Captain.Common;
using static Captain.Application.Application;

namespace Captain.Application {
  /// <inheritdoc />
  /// <summary>
  ///   Copies still captures to the clipboard for supported formats
  /// </summary>
  [Common.DisplayName("Copy to Clipboard")]
  [DisplayIcon(typeof(Resources), nameof(Resources.ClipboardHandlerExtensionIcon))]
  internal class ClipboardHandler : Handler {
    /// <summary>
    ///   Mimetypes supported by the clipboard
    /// </summary>
    private static readonly string[] SupportedMimetypes =
      { "image/png", "image/gif", "image/jpeg", "image/pjpeg", "image/bmp", "image/x-windows-bmp" };

    /// <inheritdoc />
    /// <summary>
    ///   Class constructor
    /// </summary>
    /// <param name="workflow">Workflow originating the capture</param>
    /// <param name="codec">Capture codec</param>
    /// <param name="stream">Capture stream</param>
    /// <param name="options">Optional user-provided options</param>
    /// <exception cref="NotSupportedException">
    ///   Thrown When the mimetype is not provided by the codec or is not supported
    /// </exception>
    public ClipboardHandler(WorkflowBase workflow, Codec codec, Stream stream, object options)
      : base(workflow, codec, stream, options) {
      if (!SupportedMimetypes.Contains(codec.GetMediaType()?.Type)) {
        throw new NotSupportedException("The media type is unknown or not supported by this handler");
      }
    }

    /// <inheritdoc />
    /// <summary>
    ///   Handles captures
    /// </summary>
    public override void Handle() {
      var thread = new Thread(() => {
        using (Image image = Image.FromStream(Stream, true, true)) {
          Clipboard.SetImage(image);
        }
      });
      Log.Trace("created thread for clipboard image");

      if (!thread.TrySetApartmentState(ApartmentState.STA)) {
        throw new InvalidOperationException("Could not set single-threaded apartme state for thread");
      }

      // start and join thread so that we don't get any race and our stream ends up disposed
      Log.Trace("successfully set STA apartment state");
      thread.Start();
      thread.Join();
      Log.Trace("thread exited");
    }
  }
}