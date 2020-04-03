using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Captain.Common.Native;
using SharpDX.DXGI;

namespace Captain.Application {
  /// <summary>
  ///   Contains diverse utility methods for working with displays and output devices
  /// </summary>
  internal static class DisplayHelper {
    /// <summary>
    ///   Gets adapter/output indices from a given virtual desktop rectangle
    /// </summary>
    /// <param name="rect">The rectangle</param>
    /// <returns>
    ///   A triplet containing the adapter and output indices and the bounds that intersect with their regions
    /// </returns>
    internal static (int AdapterIndex, int OutputIndex, Rectangle Bounds)[] GetOutputInfoFromRect(Rectangle rect) {
      using (var factory = new Factory1()) {
        var triples = new List<(int, int, Rectangle)>();
        int adapterIndex = 0;

        if (factory.GetAdapterCount1() == 0) {
          // no usable adapters - retrieve virtual desktop information
          int outputIndex = 0;
          foreach (Screen screen in Screen.AllScreens) {
            // calculate intersection
            Rectangle intersection = Rectangle.Intersect(rect, screen.Bounds);

            // make sure the rectangles intersect
            if (intersection != Rectangle.Empty) {
              triples.Add((adapterIndex, outputIndex, intersection));
            }

            outputIndex++;
          }

          return triples.ToArray();
        }

        // enumerate outputs
        foreach (Adapter1 adapter in factory.Adapters1) {
          int outputIndex = 0;

          foreach (Output output in adapter.Outputs) {
            // convert to Rectangle
            var outputRect = new Rectangle(output.Description.DesktopBounds.Left,
                                           output.Description.DesktopBounds.Top,
                                           output.Description.DesktopBounds.Right -
                                           output.Description.DesktopBounds.Left,
                                           output.Description.DesktopBounds.Bottom -
                                           output.Description.DesktopBounds.Top);

            // calculate intersection
            Rectangle intersection = Rectangle.Intersect(rect, outputRect);

            // make sure the rectangles intersect
            if (intersection != Rectangle.Empty) {
              triples.Add((adapterIndex, outputIndex, intersection));
            }

            outputIndex++;
            output.Dispose();
          }

          adapterIndex++;
          adapter.Dispose();
        }

        return triples.ToArray();
      }
    }

    /// <summary>
    ///   Gets the DPI value for the default display or the screen containing the specified window
    /// </summary>
    /// <param name="hwnd">Optionally specify a window handle</param>
    /// <returns>A float value containing the screen DPI</returns>
    internal static float GetScreenDpi(IntPtr? hwnd = null) {
      try {
        // return system DPI/DPI for the specified window
        return hwnd.HasValue ? User32.GetDpiForWindow(hwnd.Value) : User32.GetDpiForSystem();
      } catch (EntryPointNotFoundException) {
        // unsupported platform
        using (Graphics graphics = Graphics.FromHwnd(hwnd ?? User32.GetDesktopWindow())) {
          return graphics.DpiX;
        }
      }
    }
  }
}