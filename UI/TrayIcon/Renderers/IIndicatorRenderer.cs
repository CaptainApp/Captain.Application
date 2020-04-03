using System.Drawing;

namespace Captain.Application {
  /// <summary>
  ///   Renders indicator icons
  /// </summary>
  internal interface IIndicatorRenderer {
    /// <summary>
    ///   Renders a single tray icon frame
    /// </summary>
    /// <param name="status">Icon status</param>
    /// <returns>An <see cref="Icon" /> instance</returns>
    Icon RenderFrame(IndicatorStatus status);
  }
}