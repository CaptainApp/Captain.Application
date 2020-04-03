// TODO: refactor and comment this file
// ReSharper disable All

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Captain.Common.Native;

namespace Captain.Application {
  internal sealed class LinkLabel2 : Control {
    private Font hoverFont;

    private bool isHovered;
    private bool keyAlreadyProcessed;
    private Rectangle textRect;

    [DefaultValue(true)]
    private bool HoverUnderline { get; }

    [DefaultValue(true)]
    public bool UseSystemColor { get; set; }

    public Color RegularColor { get; set; }
    public Color HoverColor { get; set; }

    public override string Text {
      get => base.Text;
      set {
        base.Text = value;
        RefreshTextRect();
        Invalidate();
      }
    }

    public LinkLabel2() {
      if (!DesignMode) {
        SetStyle(ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.SupportsTransparentBackColor |
                 ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw |
                 ControlStyles.UserPaint |
                 ControlStyles.FixedHeight |
                 ControlStyles.FixedWidth,
                 true);

        SetStyle(ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, false);

        this.hoverFont = new Font(Font, FontStyle.Underline);

        ForeColor = SystemColors.HotTrack;

        UseSystemColor = true;
        HoverUnderline = true;
      }
    }

    protected override void OnMouseDown(MouseEventArgs e) {
      if (e.Button == MouseButtons.Left) {
        Focus();
      }

      base.OnMouseDown(e);
    }

    protected override void OnMouseEnter(EventArgs e) {
      this.isHovered = true;
      Invalidate();

      base.OnMouseEnter(e);
    }

    protected override void OnMouseLeave(EventArgs e) {
      this.isHovered = false;
      Invalidate();

      base.OnMouseLeave(e);
    }

    protected override void OnMouseMove(MouseEventArgs mevent) {
      base.OnMouseMove(mevent);
      if (mevent.Button == MouseButtons.None) {
        return;
      }

      if (!ClientRectangle.Contains(mevent.Location)) {
        if (!this.isHovered) {
          return;
        }

        this.isHovered = false;
        Invalidate();
      } else if (!this.isHovered) {
        this.isHovered = true;
        Invalidate();
      }
    }

    protected override void OnGotFocus(EventArgs e) {
      Invalidate();

      base.OnGotFocus(e);
    }

    protected override void OnLostFocus(EventArgs e) {
      this.keyAlreadyProcessed = false;
      Invalidate();

      base.OnLostFocus(e);
    }

    protected override void OnKeyDown(KeyEventArgs e) {
      if (!this.keyAlreadyProcessed && e.KeyCode == Keys.Enter) {
        this.keyAlreadyProcessed = true;
        OnClick(e);
      }

      base.OnKeyDown(e);
    }

    protected override void OnKeyUp(KeyEventArgs e) {
      this.keyAlreadyProcessed = false;

      base.OnKeyUp(e);
    }

    protected override void OnMouseUp(MouseEventArgs e) {
      if (this.isHovered && e.Clicks == 1 && (e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle)) {
        OnClick(e);
      }

      base.OnMouseUp(e);
    }

    protected override void OnPaint(PaintEventArgs e) {
      e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
      e.Graphics.InterpolationMode = InterpolationMode.Low;

      // text
      TextRenderer.DrawText(e.Graphics,
                            Text,
                            this.isHovered && HoverUnderline ? this.hoverFont : Font,
                            this.textRect,
                            UseSystemColor ? ForeColor : (this.isHovered ? HoverColor : RegularColor),
                            TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);

      // draw the focus rectangle
      if (Focused && ShowFocusCues) {
        ControlPaint.DrawFocusRectangle(e.Graphics, ClientRectangle);
      }
    }

    protected override void OnFontChanged(EventArgs e) {
      this.hoverFont = new Font(Font, Font.Style | FontStyle.Underline);
      RefreshTextRect();

      base.OnFontChanged(e);
    }

    private void RefreshTextRect() =>
      this.textRect = new Rectangle(Point.Empty,
                                    Size = Size.Add(TextRenderer.MeasureText(Text, this.hoverFont, Size),
                                                    new Size(1, 1)));

    protected override void WndProc(ref Message m) {
      if (!DesignMode) {
        if (m.Msg == (int) User32.WindowMessage.WM_SETCURSOR) {
          User32.SetCursor(User32.LoadCursor(IntPtr.Zero, new IntPtr((int) User32.OCR_HAND)));
          m.Result = IntPtr.Zero;
          return;
        }
      }

      base.WndProc(ref m);
    }
  }
}