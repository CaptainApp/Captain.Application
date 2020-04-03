using System;
using System.Text;
using System.Windows.Forms;
using Captain.Common.Native;

namespace Captain.Application {
  internal static class KeysExtensions {
    /// <summary>
    ///   Obtains the human-readable hotkey string for the key data
    /// </summary>
    /// <param name="keyData">Key data</param>
    /// <returns>A string</returns>
    internal static string ToHotkeyString(this Keys keyData) {
      if (keyData == Keys.None) {
        return String.Empty;
      }

      string humanString = "";

      if (keyData.HasFlag(Keys.Control)) {
        humanString += "CTRL + ";
      }

      if (keyData.HasFlag(Keys.Alt)) {
        humanString += "ALT + ";
      }

      if (keyData.HasFlag(Keys.Shift)) {
        humanString += "SHIFT + ";
      }

      Keys keyCode = keyData & Keys.KeyCode;
      int scanCode = User32.MapVirtualKey((int) keyCode, User32.MapType.MAPVK_VK_TO_VSC);

      if (keyCode == Keys.Left || keyCode == Keys.Up ||
          keyCode == Keys.Right || keyCode == Keys.Down ||
          keyCode == Keys.Prior || keyCode == Keys.Next ||
          keyCode == Keys.End || keyCode == Keys.Home ||
          keyCode == Keys.Insert || keyCode == Keys.Delete ||
          keyCode == Keys.Divide || keyCode == Keys.NumLock) {
        // MapVirtualKey skips the extended bit for some keys
        // see: https://www.setnode.com/blog/mapvirtualkey-getkeynametext-and-a-story-of-how-to/
        scanCode |= 0x100;
      }

      return humanString + GetActualKeyName(keyCode, scanCode);
    }

    /// <summary>
    ///   Gets the name of the specified key
    /// </summary>
    /// <param name="keyCode">Key code</param>
    /// <param name="scanCode">Scan code</param>
    /// <returns>A string</returns>
    private static string GetActualKeyName(Keys keyCode, int scanCode) {
      var keyNameBuilder = new StringBuilder(32);
      User32.GetKeyNameText(scanCode << 16,
                            keyNameBuilder,
                            keyNameBuilder.Capacity - 1);

      string keyName = keyNameBuilder.ToString().ToUpper();
      if (String.IsNullOrEmpty(keyName)) {
        keyName = keyCode.ToString().ToUpper();
      }

      return keyName;
    }
  }
}