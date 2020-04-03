using System;
using System.Drawing;
using System.Windows.Forms;
using Captain.Common.Native;
using Transitions;

namespace Captain.Application {
  /// <inheritdoc />
  /// <summary>
  ///   Contains code for abstracting application window features
  /// </summary>
  internal class Window : Form {
    /// <summary>
    ///   Whether to paint a title bar border on the top of the window
    /// </summary>
    private readonly bool paintTitleBarBorder;

    /// <summary>
    ///   Last DPI value for this form
    /// </summary>
    private float lastDpi = 96;

    /// <inheritdoc />
    /// <summary>Gets the required creation parameters when the control handle is created.</summary>
    /// <returns>
    ///   A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the
    ///   handle to the control is created.
    /// </returns>
    protected override CreateParams CreateParams {
      get {
        CreateParams createParams = base.CreateParams;
        if (!DesignMode) {
          createParams.ExStyle |= (int) User32.WindowStylesEx.WS_EX_COMPOSITED;
        }

        return createParams;
      }
    }

    /// <inheritdoc />
    /// <summary>
    ///   Class constructor
    /// </summary>
    internal Window() {
#if !DEBUG
      if (!DesignMode) {
        try {
          // focus the existing instance of the form instead of creating yet another one
          System.Windows.Forms.Application.OpenForms.Cast<Form>().First(f => f.GetType() == GetType()).Focus();
          throw new ApplicationException("It is not allowed to create two simultaneous instances of this window.");
        } catch (InvalidOperationException) {
          /* no form for us */
        }
      }
      #endif

      AutoScaleMode = AutoScaleMode.Font;
      StartPosition = FormStartPosition.CenterScreen;

      // draw title bar border on Windows 10 and upwards, so the little contrast between the white title bar and the
      // light gray background is not noticed
      this.paintTitleBarBorder = Environment.OSVersion.Version.Major >= 10;
    }

    /// <summary>
    ///   Resizes the window with an animation
    /// </summary>
    /// <param name="newSize">New size</param>
    protected void AnimateResize(Size newSize) {
      var transition = new Transition(new TransitionType_EaseInEaseOut(150));
      transition.add(this, "Width", newSize.Width);
      transition.add(this, "Height", newSize.Height);
      transition.run();
    }

    /// <summary>
    ///   Updates the DPI setting for this form
    /// </summary>
    /// <param name="dpi">New DPI value</param>
    private void UpdateDpi(float dpi) {
      Font = new Font(Font.FontFamily, Font.Size * (dpi / this.lastDpi));
      this.lastDpi = dpi;
    }

    /// <inheritdoc />
    /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
    /// <param name="eventArgs">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected override void OnHandleCreated(EventArgs eventArgs) {
      DoubleBuffered = true;
      SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);

      if (Environment.OSVersion.Version.Major < 10) {
        // static scaling for Windows < 10
        this.lastDpi = DisplayHelper.GetScreenDpi(Handle);
        Scale(new SizeF(this.lastDpi / 96, this.lastDpi / 96));
      }

      base.OnHandleCreated(eventArgs);
    }

    /// <inheritdoc />
    /// <summary>
    ///   Sets the initial DPI for this form
    /// </summary>
    /// <param name="eventArgs">Event arguments</param>
    protected override void OnLoad(EventArgs eventArgs) {
      if (!DesignMode) {
        if (Environment.OSVersion.Version.Major >= 10) {
          UpdateDpi(DisplayHelper.GetScreenDpi(Handle));
        }

        // restore saved window position, if any
        if (Application.Options.WindowPositions.ContainsKey(Name)) {
          // got saved position
          Point position = Application.Options.WindowPositions[Name];

          if (DisplayHelper.GetOutputInfoFromRect(new Rectangle(position, Size)).Length > 0) {
            // the window would be visible - no problem
            StartPosition = FormStartPosition.Manual;
            Location = position;
          }
        }
      }

      base.OnLoad(eventArgs);
    }

    /// <inheritdoc />
    /// <summary>
    ///   Save window position
    /// </summary>
    /// <param name="eventArgs">Event arguments</param>
    protected override void OnClosed(EventArgs eventArgs) {
      Application.Options.WindowPositions[Name] = Location;
      Application.Options.Save();
      base.OnClosed(eventArgs);
    }

    /// <inheritdoc />
    /// <summary>Paints the background of the control.</summary>
    /// <param name="eventArgs">
    ///   A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.
    /// </param>
    protected override void OnPaint(PaintEventArgs eventArgs) {
      if (this.paintTitleBarBorder) {
        eventArgs.Graphics.DrawLine(new Pen(Color.FromArgb(0x20, 0, 0, 0)), 0, 0, Width, 0);
      }

      base.OnPaint(eventArgs);
    }

    /// <inheritdoc />
    /// <summary>
    ///   Window procedure override for handling DPI changes
    /// </summary>
    /// <param name="msg">Window message</param>
    protected override void WndProc(ref Message msg) {
      if (msg.Msg == (int) User32.WindowMessage.WM_DPICHANGED) {
        UpdateDpi((msg.WParam.ToInt64() >> 16) & 0xFFFF);
      }

      base.WndProc(ref msg);
    }
  }
}