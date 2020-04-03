using System.IO;
using Aperture;
using Captain.Common;
using SharpDX.WIC;
using static Captain.Application.Application;

namespace Captain.Application {
  /// <inheritdoc />
  /// <summary>
  ///   Implements a WIC-enabled PNG image codec
  /// </summary>
  [Aperture.DisplayName("PNG")]
  [MediaType("image/png", "png")]
  [OptionProvider(typeof(CustomOptionProvider))]
  internal sealed class PngWicStillImageCodec : WicStillImageCodec {
    /// <inheritdoc />
    /// <summary>
    ///   Defines options logic for this plugin
    /// </summary>
    private class CustomOptionProvider : IOptionProvider {
      /// <inheritdoc />
      /// <summary>
      ///   Displays the UI for configuring this plugin
      /// </summary>
      /// <param name="options">The current plugin options</param>
      /// <returns>The new options for this plugin</returns>
      public object DisplayOptionUi(object options) {
        Log.Info("Hello, world!");
        return options;
      }

      /// <inheritdoc />
      /// <summary>
      ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
      /// </summary>
      public void Dispose() { }
    }

    /// <inheritdoc />
    /// <summary>
    ///   Class constructor
    /// </summary>
    /// <param name="width">Capture width, in pixels</param>
    /// <param name="height">Capture height, in pixels</param>
    /// <param name="destStream">Destination stream</param>
    public PngWicStillImageCodec(int width, int height, Stream destStream) : base(
      width, height, destStream, ContainerFormatGuids.Png) {
      /* that's all folks! */
    }
  }
}