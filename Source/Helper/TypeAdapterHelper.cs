using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Captain.Common;
using DisplayName = Aperture.DisplayName;

namespace Captain.Application {
  /// <summary>
  ///   Type helper for ComboBox
  /// </summary>
  internal static class TypeAdapterHelper {
    /// <summary>
    ///   Return the friendly name of the type as formatted for a combo box
    ///   relying on the Description attribute containing the display value
    /// </summary>
    /// <param name="types">Type list</param>
    /// <returns>The collection of types and description fields</returns>
    public static ICollection<ComboBoxLoader<string>> GetComboBoxAdapter(IEnumerable<Type> types) {
      ICollection<ComboBoxLoader<string>> result = new List<ComboBoxLoader<string>>();

      foreach (Type type in types) {
        try {
          DisplayName displayName =
            type.GetCustomAttributes(typeof(DisplayName), false)
                .Cast<DisplayName>()
                .OrderByDescending(dn => String.Equals(dn.LanguageCode,
                                                       CultureInfo.CurrentUICulture.TwoLetterISOLanguageName,
                                                       StringComparison.OrdinalIgnoreCase))
                .First();
          result.Add(new ComboBoxLoader<string> {
            Display = displayName.Name,
            Value = type.FullName
          });
        } catch {
          result.Add(new ComboBoxLoader<string> {
            Display = type.Name,
            Value = type.FullName
          });
        }
      }

      return result;
    }
  }
}