using System;
using System.Collections.Generic;
using System.Linq;
using Captain.Common;

namespace Captain.Application {
  /// <summary>
  ///   Enumeration helper for ComboBox
  /// </summary>
  internal static class EnumAdapterHelper {
    /// <summary>
    ///   Return the contents of the enumeration as formatted for a combo box
    ///   relying on the Description attribute containing the display value
    ///   within the enum definition
    /// </summary>
    /// <typeparam name="T">The type of the enum being retrieved</typeparam>
    /// <param name="extended">Whether to include extended description or not</param>
    /// <returns>The collection of enum values and description fields</returns>
    public static ICollection<ComboBoxLoader<T>> GetComboBoxAdapter<T>(bool extended = false) {
      ICollection<ComboBoxLoader<T>> result = new List<ComboBoxLoader<T>>();

      foreach (T value in Enum.GetValues(typeof(T))) {
        FriendlyName val = value.GetType().GetMember(value.ToString()).First()
                                .GetCustomAttributes(typeof(FriendlyName), false)
                                .Cast<FriendlyName>().First();

        result.Add(new ComboBoxLoader<T> {
          Display = Resources.ResourceManager.GetString(val.Name) +
                    (extended && !String.IsNullOrEmpty(val.ExtendedDescription)
                      ? $" ({Resources.ResourceManager.GetString(val.ExtendedDescription)})"
                      : ""),
          Value = value
        });
      }

      return result;
    }
  }
}