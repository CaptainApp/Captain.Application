using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Captain.Common.Native;
using Captain.UI;

namespace Captain.Application {
  /// <inheritdoc />
  /// <summary>
  ///   macOS-like toolbar for preference ("option") dialogs
  /// </summary>
  internal sealed class TabStripControl : TabControl {
    /// <summary>
    ///   Index of the tab that is currently being pressed (-1 for none)
    /// </summary>
    private int downIndex = -1;

    /// <summary>
    ///   Whether to expand tabs to fill horizontal space
    /// </summary>
    private bool extendTabs;

    /// <summary>
    ///   Index of the tab that is currently hovered (-1 for none)
    /// </summary>
    private int hoverIndex = -1;

    /// <summary>
    ///   Reference font size for 1x scale
    /// </summary>
    private float referenceFontSize;

    /// <summary>
    ///   Color for the tab header
    /// </summary>
    private static Brush TabHeaderBrush => new SolidBrush(Color.FromArgb(0xFF, 0xE7, 0xE7, 0xE7));

    /// <summary>
    ///   Pen for drawing tab separators
    /// </summary>
    private static Pen BorderPen => new Pen(Color.FromArgb(0x20, Color.Black));

    /// <summary>
    ///   Brush for drawing pressed tabs' background
    /// </summary>
    private static SolidBrush PressedTabBrush => new SolidBrush(Color.FromArgb(0x20, Color.Black));

    /// <summary>
    ///   Brush for drawing hovered tabs' background
    /// </summary>
    private static SolidBrush HoveredTabBrush => new SolidBrush(Color.FromArgb(0x10, Color.Black));

    /// <summary>
    ///   Color for the tab labels
    /// </summary>
    private static Color LabelColor => Color.FromArgb(0x606060);

    /// <summary>
    ///   Pen for drawing selected tab's bottom border
    /// </summary>
    private Pen SelectedTabBorderPen { get; set; }

    /// <summary>
    ///   Color for drawing selected tabs' text
    /// </summary>
    private Color SelectedTabForeColor { get; set; }

    /// <summary>
    ///   Whether to expand tabs to fill horizontal space
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool ExtendTabs {
      // ReSharper disable once MemberCanBePrivate.Global
      get => this.extendTabs;
      set {
        this.extendTabs = value;
        UpdateItemSize();
      }
    }

    /// <inheritdoc />
    /// <summary>
    ///   Initializes a new instance of this control
    /// </summary>
    /// <remarks>
    ///   Public constructor ensures the Windows Forms designer generates code for this control
    /// </remarks>
    public TabStripControl() {
      this.referenceFontSize = Font.Size;
    }

    /// <inheritdoc />
    /// <summary>This member overrides <see cref="M:System.Windows.Forms.Control.OnHandleCreated(System.EventArgs)" />.</summary>
    /// <param name="eventArgs">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
    protected override void OnHandleCreated(EventArgs eventArgs) {
      if (DesignMode) {
        base.OnHandleCreated(eventArgs);
        return;
      }
      
      // set control styles so we have full control over the rendering and mouse handling procedures and ensure the
      // control is double buffered to reduce flicker
      SetStyle(ControlStyles.UserPaint |
               ControlStyles.UserMouse |
               ControlStyles.AllPaintingInWmPaint |
               ControlStyles.DoubleBuffer |
               ControlStyles.OptimizedDoubleBuffer |
               ControlStyles.ResizeRedraw,
               true);
      UpdateAccentColor();
      UpdateItemSize();

      // HACK: hook onto the parent control events so that we can "see" through the actually transparent region on the
      //       tab control (see https://stackoverflow.com/a/39331477/2541873)
      Parent.MouseMove += (s, e) => OnMouseMove(e);
      Parent.MouseLeave += (s, e) => OnMouseLeave(e);
      Parent.MouseDown += (s, e) => OnMouseDown(e);
      Parent.MouseUp += (s, e) => OnMouseUp(e);

      base.OnHandleCreated(eventArgs);
    }

    /// <inheritdoc />
    /// <summary>
    ///   This member overrides <see cref="M:System.Windows.Forms.Control.WndProc(System.Windows.Forms.Message@)" />.
    /// </summary>
    /// <param name="m">A Windows Message Object. </param>
    protected override void WndProc(ref Message m) {
      if (Disposing || DesignMode) {
        base.WndProc(ref m);
        return;
      }

      if (m.Msg == (int) User32.WindowMessage.TCM_ADJUSTRECT) {
        var rect = (RECT) m.GetLParam(typeof(RECT));

        rect.top -= 3;
        rect.right += 4;
        rect.bottom += 4;
        rect.left -= 4;

        Marshal.StructureToPtr(rect, m.LParam, true);
      }

      base.WndProc(ref m);
    }

    /// <inheritdoc />
    /// <summary>
    ///   Scale tabs accordingly when font changes
    /// </summary>
    /// <param name="eventArgs">Arguments passed to this event handler</param>
    protected override void OnFontChanged(EventArgs eventArgs) {
      if (DesignMode) {
        base.OnFontChanged(eventArgs);
        return;
      }

      decimal scale = (decimal) Font.Size / (decimal) this.referenceFontSize;
      this.referenceFontSize = Font.Size;
      ItemSize = new Size((int) Math.Floor(ItemSize.Width * scale), (int) Math.Floor(ItemSize.Height * scale));
      UpdateItemSize();

      base.OnFontChanged(eventArgs);
    }

    /// <summary>
    ///   Updates accent colors
    /// </summary>
    internal void UpdateAccentColor() {
      // set accent color
      SelectedTabBorderPen = new Pen(StyleHelper.GetAccentColor() ?? Color.FromArgb(0x40, Color.Black));
      SelectedTabForeColor = SelectedTabBorderPen.Color.Blend(Color.Black);
      Invalidate(true);
    }

    /// <summary>
    ///   Updates tab size
    /// </summary>
    internal void UpdateItemSize() {
      if (ExtendTabs && Width > 0 && TabCount > 0) {
        ItemSize = new Size(Width / TabCount, ItemSize.Height);
        Invalidate(true);
      }
    }

    /// <summary>
    ///   Gets the Rectangle that is to be occupied for the tab with the specified index
    /// </summary>
    /// <param name="index">The zero-based tab index</param>
    /// <returns>A Rectangle representing the tab bounds</returns>
    private Rectangle GetTabBounds(int index) {
      var bounds = new Rectangle(ItemSize.Width * index, 0, ItemSize.Width, ItemSize.Height);

      if (ExtendTabs && index == TabCount - 1) {
        bounds.Width = Width - bounds.X;
      }

      return bounds;
    }

    /// <inheritdoc />
    /// <summary>
    ///   Clears background
    /// </summary>
    /// <param name="eventArgs">Arguments passed to this event handler</param>
    protected override void OnPaintBackground(PaintEventArgs eventArgs) {
      if (DesignMode) {
        base.OnPaintBackground(eventArgs);
        return;
      }

      // clear control with the default background color
      eventArgs.Graphics.Clear(Color.WhiteSmoke);

      // clear header with darker color
      eventArgs.Graphics.FillRectangle(TabHeaderBrush, new Rectangle(0, 0, Width, ItemSize.Height));

      // draw header border
      eventArgs.Graphics.DrawLine(BorderPen, Left, ItemSize.Height, Right, ItemSize.Height);
    }

    /// <inheritdoc />
    /// <summary>
    ///   Triggered when the mouse leaves the tab header
    /// </summary>
    /// <param name="eventArgs">Event arguments</param>
    protected override void OnMouseLeave(EventArgs eventArgs) {
      if (DesignMode) {
        base.OnMouseLeave(eventArgs);
        return;
      }

      if (this.hoverIndex != -1) {
        // invalidate the currently hovered tab, if any
        Invalidate(GetTabBounds(this.hoverIndex));

        // tab no longer hovered
        this.hoverIndex = -1;
      }
    }

    /// <inheritdoc />
    /// <summary>
    ///   Triggered when the mouse moves around the tab header area
    /// </summary>
    /// <param name="eventArgs"></param>
    protected override void OnMouseMove(MouseEventArgs eventArgs) {
      if (DesignMode) {
        base.OnMouseMove(eventArgs);
        return;
      }

      int previousHoverIndex = this.hoverIndex; // previously hovered tab index
      this.hoverIndex = -1;                     // reset hover index

      // mouse is moving around the tab area, calculate the tab index from its horizontal position
      this.hoverIndex = eventArgs.X / ItemSize.Width;

      if (this.hoverIndex != previousHoverIndex) {
        // the hovered tab has changed, invalidate previous and current tab regions
        Invalidate(GetTabBounds(previousHoverIndex));
        Invalidate(GetTabBounds(this.hoverIndex));
      }
    }

    /// <inheritdoc />
    /// <summary>
    ///   Triggered when the user holds the primary mouse button
    /// </summary>
    /// <param name="eventArgs">Event arguments</param>
    protected override void OnMouseDown(MouseEventArgs eventArgs) {
      if (DesignMode) {
        base.OnMouseDown(eventArgs);
        return;
      }

      if (eventArgs.Button == MouseButtons.Left) {
        this.downIndex = eventArgs.X / ItemSize.Width;
        Invalidate(GetTabBounds(this.downIndex));
      }
    }

    /// <inheritdoc />
    /// <summary>
    ///   Triggered when the user releases the primary mouse button
    /// </summary>
    /// <param name="eventArgs">Event arguments</param>
    protected override void OnMouseUp(MouseEventArgs eventArgs) {
      if (DesignMode) {
        base.OnMouseUp(eventArgs);
        return;
      }

      if (this.downIndex != -1) {
        SelectedIndex = this.downIndex;
        this.downIndex = -1;
        Invalidate();
      }
    }

    /// <inheritdoc />
    /// <summary>
    ///   Paint procedure
    /// </summary>
    /// <param name="eventArgs">Event arguments</param>
    protected override void OnPaint(PaintEventArgs eventArgs) {
      if (DesignMode) {
        base.OnPaint(eventArgs);
        return;
      }

      eventArgs.Graphics.DrawLine(new Pen(Color.FromArgb(0x20, 0, 0, 0)), 0, 0, Width, 0);

      try {
        for (int i = 0; i < TabCount; i++) {
          // get bounds for the tab being rendered
          Rectangle tabBounds = GetTabBounds(i);

          if (this.downIndex == i
          ) {
            eventArgs.Graphics.FillRectangle(PressedTabBrush, tabBounds);
          } else if (this.hoverIndex == i) {
            eventArgs.Graphics.FillRectangle(HoveredTabBrush, tabBounds);
          }

          if (i != TabCount - 1) {
            eventArgs.Graphics.DrawLine(BorderPen,
                                        tabBounds.Right,
                                        tabBounds.Top + 4,
                                        tabBounds.Right,
                                        tabBounds.Bottom - 4);
          }

          // render tab label
          TextRenderer.DrawText(eventArgs.Graphics,
                                TabPages[i].Text,
                                Font,
                                new Rectangle(tabBounds.X, tabBounds.Y, tabBounds.Width, tabBounds.Height),
                                SelectedIndex == i ? SelectedTabForeColor : LabelColor,
                                TextFormatFlags.EndEllipsis |
                                TextFormatFlags.HorizontalCenter |
                                TextFormatFlags.VerticalCenter);

          if (SelectedIndex == i) {
            eventArgs.Graphics.DrawLine(SelectedTabBorderPen,
                                        tabBounds.Left,
                                        tabBounds.Bottom,
                                        tabBounds.Right,
                                        tabBounds.Bottom);
          }
        }
      } catch (ArgumentException) { }
    }
  }
}