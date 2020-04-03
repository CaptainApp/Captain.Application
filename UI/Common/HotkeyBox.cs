using System;
using System.Drawing;
using System.Windows.Forms;
using Captain.Common.Native;

namespace Captain.Application {
  /// <inheritdoc />
  /// <summary>
  ///   Presents the user an interface for capturing a hotkey
  /// </summary>
  internal class HotkeyBox : TextBox {
    /// <summary>
    ///   Current hotkey
    /// </summary>
    private Keys hotkey = Keys.None;

    /// <summary>
    ///   Current hotkey
    /// </summary>
    internal Keys Hotkey {
      get => this.hotkey;
      set {
        this.hotkey = value;
        Text = value.ToHotkeyString();

        if (Controls.Count > 0) {
          // only display the reset button when needed
          Controls[0].Visible = this.hotkey != Keys.None;
        }
      }
    }

    /// <inheritdoc />
    /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.GotFocus" /> event.</summary>
    /// <param name="eventArgs">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected override void OnGotFocus(EventArgs eventArgs) {
      Application.DesktopKeyboardHook.RequestLock();
      Application.DesktopKeyboardHook.OnKeyDown += OnHookKeyDown;
      base.OnGotFocus(eventArgs);
    }

    /// <summary>
    ///   Triggered when a key is down, system-wide
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="eventArgs">Event arguments</param>
    private void OnHookKeyDown(object sender, KeyEventArgs eventArgs) {
      if (eventArgs.KeyCode != Keys.None) {
        Hotkey = eventArgs.KeyData;
      }
    }

    /// <inheritdoc />
    /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.LostFocus" /> event.</summary>
    /// <param name="eventArgs">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
    protected override void OnLostFocus(EventArgs eventArgs) {
      Application.DesktopKeyboardHook.OnKeyDown -= OnHookKeyDown;
      Application.DesktopKeyboardHook.RequestUnlock();
      base.OnLostFocus(eventArgs);
    }

    /// <inheritdoc />
    /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
    /// <param name="eventArgs">The event data.</param>
    protected override void OnHandleCreated(EventArgs eventArgs) {
      base.OnHandleCreated(eventArgs);

      // don't let the text overflow the visible section
      User32.SendMessage(Handle,
                         (int) User32.WindowMessage.EM_SETMARGINS,
                         (IntPtr) User32.EditMarginValues.EC_RIGHTMARGIN,
                         (IntPtr) (24 << 16));

      ReadOnly = true;
      BackColor = SystemColors.Window;

      Controls.Add(new LinkButton {
        Bounds = Environment.OSVersion.Version >= new Version(6, 2)
          ? new Rectangle(ClientSize.Width - 19, -4, 19, 27) // rounded corners are not visible on Windows >= 8
          : new Rectangle(ClientSize.Width - 19, 0, 19, 19), // rounded corners are fully visible
        Image = Resources.EraseField,
        Visible = false
      });

      Controls[0].Click += delegate { Hotkey = Keys.None; };
      new ToolTip().SetToolTip(Controls[0], Resources.HotkeyBox_ResetToolTipText);
    }

    /// <inheritdoc />
    /// <summary>Processes Windows messages.</summary>
    /// <param name="msg">A Windows Message object. </param>
    protected override void WndProc(ref Message msg) {
      if (msg.Msg == (int) User32.WindowMessage.WM_CHAR) {
        // remove blinking cursor
        User32.HideCaret(Handle);
      }

      base.WndProc(ref msg);
    }
  }
}