using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Captain.UI;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
namespace Captain.Application {
  /// <inheritdoc />
  /// <summary>
  ///   Extended label which allows for ellipsis animations
  /// </summary>
  internal sealed class EllipsisProgressLabel : Label {
    /// <summary>
    ///   Animation frames
    /// </summary>
    private readonly string[] frames = { "...", "·..", ".·.", "..·", "..." };

    /// <summary>
    ///   Whether or not the label is animated
    /// </summary>
    private bool animated;

    /// <summary>
    ///   Animation thread
    /// </summary>
    private Thread animationThread;

    /// <summary>
    ///   Label prefix (actual text)
    /// </summary>
    private string prefix;

    /// <summary>
    ///   Label suffix
    /// </summary>
    private string suffix;

    /// <summary>
    ///   Suffix to be appended after the ellipsis
    /// </summary>
    public string Prefix {
      get => this.prefix;
      set {
        this.prefix = value;
        Text = this.prefix + (ShowEllipsis ? this.frames[0] : "");
        Refresh();
      }
    }

    /// <summary>
    ///   Suffix to be appended after the ellipsis
    /// </summary>
    public string Suffix {
      get => this.suffix;
      set {
        this.suffix = value;
        Refresh();
      }
    }

    /// <summary>
    ///   Whether or not the label is animated
    /// </summary>
    public bool Animated {
      get => this.animated;
      set {
        if (DesignMode) { this.animated = value; } else if (value && !this.animated) {
          this.animationThread?.Join();
          this.animationThread = new Thread(() => {
            byte i = 0;

            while (this.animated) {
              if (IsHandleCreated && Visible && !IsDisposed) {
                Invoke(new Action(() => Text = Prefix + this.frames[i = (byte) ((i + 1) % this.frames.Length)]));
              }

              Thread.Sleep(60);
            }
          });

          this.animated = true;
          this.animationThread.Start();
        } else if (!value && this.animated) {
          this.animated = false;
          this.animationThread?.Join();
          this.animationThread = null;
        }
      }
    }

    /// <summary>
    ///   Whether or not to show ellipsis when not animating
    /// </summary>
    public bool ShowEllipsis { get; set; } = true;

    /// <inheritdoc />
    /// <summary>
    ///   Paints the control
    /// </summary>
    /// <param name="eventArgs">Event arguments</param>
    protected override void OnPaint(PaintEventArgs eventArgs) {
      // measure text (prefix + ellipsis) width
      int textWidth = TextRenderer.MeasureText(Text, Font).Width;

      // draw text
      TextRenderer.DrawText(eventArgs.Graphics, Text, Font, Point.Empty, ForeColor);

      // draw suffix
      TextRenderer.DrawText(eventArgs.Graphics,
        Suffix,
        Font,
        new Point(textWidth, 0),
        ForeColor.Blend(BackColor));
    }
  }
}