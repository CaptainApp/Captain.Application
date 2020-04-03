using System;

namespace Captain.Application {
  /// <summary>
  ///   Implements methods to manipulate integer bits
  /// </summary>
  internal static class BitHelper {
    /// <summary>
    ///   Creates a LONG value by concatenating the specified values.
    /// </summary>
    /// <param name="lo">The low-order word of the new value.</param>
    /// <param name="hi">The high-order word of the new value.</param>
    /// <returns>The return value is a LONG value. </returns>
    internal static int MakeLong(int lo, int hi) => LoWord(lo) | (short) (0x10000 * LoWord(hi));

    /// <summary>
    ///   Retrieves the low-order word from the specified value.
    /// </summary>
    /// <param name="dword">The value to be converted.</param>
    /// <returns>The return value is the low-order word of the specified value.</returns>
    private static short LoWord(int dword) => (short) (dword & Int16.MaxValue);
  }
}