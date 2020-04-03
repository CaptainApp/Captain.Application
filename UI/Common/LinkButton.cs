using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Captain.Common.Native;
using Captain.UI;

// ReSharper disable MemberCanBePrivate.Global

namespace Captain.Application {
  /// <inheritdoc />
  /// <summary>
  ///   Represents a button control with no background
  /// </summary>
  internal sealed class LinkButton : Control {
    /// <summary>
    ///   Indicates whether the mouse button is pressed or not
    /// </summary>
    private bool mouseDown;

    /// <summary>
    ///   Indicates whether the mouse is over the control or not
    /// </summary>
    private bool mouseOver;

    /// <summary>
    ///   Specifies a tint color for this control
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color TintColor { get; set; } = Color.Transparent;

    /// <summary>
    ///   Specifies an image to be displayed alongside this control
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Bitmap Image { get; set; }

    /// <inheritdoc />
    /// <summary>
    ///   Class constructor.
    /// </summary>
    public LinkButton() {
      SetStyle(ControlStyles.SupportsTransparentBackColor |
               ControlStyles.UserPaint |
               ControlStyles.AllPaintingInWmPaint |
               ControlStyles.DoubleBuffer |
               ControlStyles.StandardClick,
        true);
    }

    /// <inheritdoc />
    /// <summary>Processes Windows messages.</summary>
    /// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
    protected override void WndProc(ref Message m) {
      if (!DesignMode) {
        if (m.Msg == (int) User32.WindowMessage.WM_SETCURSOR) {
          User32.SetCursor(User32.LoadCursor(IntPtr.Zero, new IntPtr(User32.OCR_HAND)));
          m.Result = IntPtr.Zero;
          return;
        }
      }

      base.WndProc(ref m);
    }

    /// <inheritdoc />
    /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseEnter" /> event.</summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
    protected override void OnMouseEnter(EventArgs e) {
      this.mouseOver = true;
      Invalidate();
      base.OnMouseEnter(e);
    }

    /// <inheritdoc />
    /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave" /> event.</summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
    protected override void OnMouseLeave(EventArgs e) {
      this.mouseOver = false;
      Invalidate();
      base.OnMouseLeave(e);
    }

    /// <inheritdoc />
    /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.</summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
    protected override void OnMouseDown(MouseEventArgs e) {
      this.mouseDown = true;
      Invalidate();
      base.OnMouseDown(e);
    }

    /// <inheritdoc />
    /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseUp" /> event.</summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
    protected override void OnMouseUp(MouseEventArgs e) {
      this.mouseDown = false;
      Invalidate();
      base.OnMouseUp(e);
    }

    /// <inheritdoc />
    /// <summary>
    ///   Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnPaint(System.Windows.Forms.PaintEventArgs)" />
    ///   event.
    /// </summary>
    /// <param name="eventArgs">
    ///   A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.
    /// </param>
    protected override void OnPaint(PaintEventArgs eventArgs) {
      eventArgs.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
      eventArgs.Graphics.CompositingQuality = CompositingQuality.HighQuality;
      eventArgs.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
      eventArgs.Graphics.SmoothingMode = SmoothingMode.HighQuality;

      using (var path = new GraphicsPath()) {
        const int diameter = 4;
        var arc = new Rectangle(0, 0, diameter, diameter);

        path.AddArc(arc, 180, 90);

        arc.X = Width - diameter;
        path.AddArc(arc, 270, 90);

        arc.Y = Height - diameter;
        path.AddArc(arc, 0, 90);

        arc.X = 0;
        path.AddArc(arc, 90, 90);

        path.CloseFigure();

        if (this.mouseOver || this.mouseDown) {
          eventArgs.Graphics.FillPath(new SolidBrush(TintColor.Blend(ForeColor, this.mouseDown ? 0.5 : 0.75)
              .Blend(Color.Transparent)),
            path);
        }
      }

      int horizontalMargin = Image == null ? 0 : 4;
      int imageRectWidth = horizontalMargin + Image?.Width ?? 0;

      Point imageLocation = Point.Empty;
      if (!String.IsNullOrWhiteSpace(Text)) {
        Size textSize = TextRenderer.MeasureText(eventArgs.Graphics, Text, Font);
        imageLocation = new Point((Width - textSize.Width - (Image?.Width ?? 0)) / 2,
          (Height - Image?.Height ?? 0) / 2);

        TextRenderer.DrawText(eventArgs.Graphics,
          Text,
          Font,
          new Rectangle(imageRectWidth, 0, Width - imageRectWidth, Height),
          Enabled ? this.mouseDown ? ForeColor : TintColor.Blend(ForeColor, this.mouseOver ? 0.25 : 0.5) : Color.Gray,
          TextFormatFlags.HorizontalCenter |
          TextFormatFlags.EndEllipsis |
          TextFormatFlags.VerticalCenter);
      } else if (Image != null) { imageLocation = new Point((Width - Image.Width) / 2, (Height - Image.Height) / 2); }

      if (Image != null) {
        var attrs = new ImageAttributes();
        float n = TintColor.R + TintColor.G + TintColor.B;
        n = Math.Abs(n) < 1 ? 255 : n;

        // normalize color values
        float cr = TintColor.R / n;
        float cg = TintColor.G / n;
        float cb = TintColor.B / n;

        // alpha
        float ca = this.mouseDown ? 1f : (this.mouseOver ? 0.875f : 0.75f);

        // brightness
        float br = cr * 0.6666f;
        float bg = cg * 0.6666f;
        float bb = cb * 0.6666f;

        attrs.SetColorMatrix(new ColorMatrix(new[] {
          new[] { cr, cg, cb, 0f, 0f },
          new[] { cb, cr, cg, 0f, 0f },
          new[] { cg, cb, cr, 0f, 0f },
          new[] { 0f, 0f, 0f, ca, 0f },
          new[] { br, bg, bb, 0f, 1f }
        }));

        eventArgs.Graphics.DrawImage(Image,
          new Rectangle(imageLocation, Image.Size),
          0,
          0,
          Image.Width,
          Image.Height,
          GraphicsUnit.Pixel,
          attrs);
      }
    }
  }
}