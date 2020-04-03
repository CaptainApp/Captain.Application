using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Captain.Common;
using Captain.Common.Native;
using Captain.UI;
using static Captain.Application.Application;

namespace Captain.Application {
  /// <inheritdoc cref="Behaviour" />
  /// <summary>
  ///   Provides keyboard hooking logic for system UI
  /// </summary>
  internal sealed class DesktopKeyboardHook : Behaviour, IKeyboardHookProvider {
    // ReSharper disable once InconsistentNaming
    /// <summary>
    ///   The wParam and lParam parameters contain information about a keyboard message.
    /// </summary>
    private const int HC_ACTION = 0;

    /// <summary>
    ///   Handle for the system keyboard hook
    /// </summary>
    private IntPtr hookHandle;

    /// <summary>
    ///   Current keyboard state
    /// </summary>
    private Keys keys;

    /// <summary>
    ///   Low-level keyboard hook procedure reference so it it does not get garbage collected
    /// </summary>
    private User32.WindowsHookDelegate lowLevelKeyboardHook;

    /// <inheritdoc />
    /// <summary>
    ///   Triggered when a key is held
    /// </summary>
    public event KeyEventHandler OnKeyDown;

    /// <inheritdoc />
    /// <summary>
    ///   Triggered when a key is released
    /// </summary>
    public event KeyEventHandler OnKeyUp;

    /// <inheritdoc />
    /// <summary>
    ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose() => Unlock();

    /// <inheritdoc />
    /// <summary>
    ///   Starts capturing keyboard events
    /// </summary>
    protected override void Lock() {
      if (this.hookHandle != IntPtr.Zero) {
        throw new InvalidOperationException("The previous hook must be released before capturing the keyboard again.");
      }

      GC.KeepAlive(this.lowLevelKeyboardHook = LowLevelKeyboardProc);
      if ((this.hookHandle =
            User32.SetWindowsHookEx(User32.WindowsHookType.WH_KEYBOARD_LL, this.lowLevelKeyboardHook)) ==
          IntPtr.Zero) {
        Log.Error($"SetWindowHookEx() failed (last error: 0x{Marshal.GetLastWin32Error():x8})");
        throw new Win32Exception(Marshal.GetLastWin32Error());
      }

      Log.Info("desktop keyboard hook locked");
    }

    /// <inheritdoc />
    /// <summary>
    ///   Releases the keyboard hook
    /// </summary>
    protected override void Unlock() {
      if (this.hookHandle != IntPtr.Zero) {
        if (!User32.UnhookWindowsHookEx(this.hookHandle)) {
          Log.Error($"UnhookWindowsHookEx() failed (last error: 0x{Marshal.GetLastWin32Error():x8}");
        }

        this.hookHandle = IntPtr.Zero; // XXX: the hook may still be present (?), in which case this is cursed
        Log.Info("desktop keyboard hook unlocked");
      }
    }

    /// <summary>
    ///   Keyboard hook procedure
    /// </summary>
    /// <param name="code">Always 0 (HC_ACTION)</param>
    /// <param name="wParam">The identifier of the keyboard message.</param>
    /// <param name="lParam">A pointer to an KBDLLHOOKSTRUCT structure.</param>
    /// <returns>Depending on <paramref name="code" /> and other conditions, it may yield different values.</returns>
    private int LowLevelKeyboardProc(int code, IntPtr wParam, IntPtr lParam) {
      bool handled = false;
      if (code == HC_ACTION) {
        // this is a keyboard event
        var eventInfo = (KBDLLHOOKSTRUCT) Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));

        switch (wParam.ToInt32()) {
          // a key is held
          case (int) User32.WindowMessage.WM_SYSKEYDOWN:
          case (int) User32.WindowMessage.WM_KEYDOWN:
            this.keys = Keys.None;

            // see: http://www.jonegerton.com/dotnet/determining-the-state-of-modifier-keys-when-hooking-keyboard-input

            if ((User32.GetKeyState((int) User32.VirtualKeys.VK_MENU) & 0x8000) != 0) {
              this.keys |= Keys.Alt;
            }

            if ((User32.GetKeyState((int) User32.VirtualKeys.VK_CONTROL) & 0x8000) != 0) {
              this.keys |= Keys.Control;
            }

            if (this.keys == Keys.None) {
              // ensure there's at least a non-shift modifier
              break;
            }

            if ((User32.GetKeyState((int) User32.VirtualKeys.VK_SHIFT) & 0x8000) != 0) {
              this.keys |= Keys.Shift;
            }

            this.keys |= (Keys) eventInfo.vkCode;

            var keyDownEventArgs = new KeyEventArgs(this.keys);

            if (keyDownEventArgs.KeyCode == Keys.LShiftKey ||
                keyDownEventArgs.KeyCode == Keys.RShiftKey ||
                keyDownEventArgs.KeyCode == Keys.LControlKey ||
                keyDownEventArgs.KeyCode == Keys.RControlKey ||
                keyDownEventArgs.KeyCode == Keys.LMenu ||
                keyDownEventArgs.KeyCode == Keys.RMenu ||
                keyDownEventArgs.KeyCode == Keys.LWin ||
                keyDownEventArgs.KeyCode == Keys.RWin) {
              break;
            }

            OnKeyDown?.Invoke(this, keyDownEventArgs);
            handled = keyDownEventArgs.Handled;

            break;

          // a key is released
          case (int) User32.WindowMessage.WM_SYSKEYUP:
          case (int) User32.WindowMessage.WM_KEYUP:
            var keyUpEventArgs = new KeyEventArgs(this.keys);
            OnKeyUp?.Invoke(this, keyUpEventArgs);
            handled = keyUpEventArgs.Handled;

            this.keys = Keys.None;

            break;
        }
      }

      // pass unprocessed messages to system
      return handled ? 0 : User32.CallNextHookEx(this.hookHandle, code, wParam, lParam);
    }
  }
}