using System;
using System.Windows.Forms;
using Captain.Common.Native;

namespace Captain.Application {
  /// <inheritdoc />
  /// <summary>
  ///   Implements a list view control with extended properties and styles
  /// </summary>
  internal sealed class ListViewEx : ListView {
    /// <inheritdoc />
    /// <summary>Gets a value indicating whether the control should display focus rectangles.</summary>
    /// <returns>true if the control should display focus rectangles; otherwise, false.</returns>
    protected override bool ShowFocusCues => false;

    /// <inheritdoc />
    /// <summary>Raises the <see cref="M:System.Windows.Forms.Control.CreateControl" /> method.</summary>
    protected override void OnCreateControl() {
      // remove border
      BorderStyle = BorderStyle.None;

      if (!DesignMode) {
        // set native look for list view items
        UxTheme.SetWindowTheme(Handle, "explorer", null);

        // remove focus cues
        User32.SendMessage(Handle,
          (uint) User32.WindowMessage.WM_CHANGEUISTATE,
          new IntPtr(BitHelper.MakeLong((int) User32.UIStateFlags.UIS_SET,
            (int) User32.UIStateFlags.UISF_HIDEFOCUS)),
          IntPtr.Zero);

        // enable double buffer to reduce flicker and display a traslucent selection box
        User32.SendMessage(Handle,
          (uint) User32.WindowMessage.LVM_SETEXTENDEDLISTVIEWSTYLE,
          new IntPtr((int) User32.WindowStylesEx.LVS_EX_DOUBLEBUFFER),
          new IntPtr((int) User32.WindowStylesEx.LVS_EX_DOUBLEBUFFER));

        // reset hot cursor
        User32.SendMessage(Handle,
          (uint) User32.WindowMessage.LVM_SETHOTCURSOR,
          IntPtr.Zero,
          Cursors.Arrow.Handle);
      }

      base.OnCreateControl();
    }
  }
}