using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Captain.UI;

namespace Captain.Application {
  /// <inheritdoc />
  /// <summary>
  ///   Manages heads-up displays
  /// </summary>
  internal class HudManager : IDisposable {
    /// <summary>
    ///   List of HUD containers
    /// </summary>
    private readonly List<HudContainerInfo> containers = new List<HudContainerInfo>();

    /// <summary>
    ///   Class constructor
    /// </summary>
    internal HudManager() {
      this.containers.Add(new HudContainerInfo {
        ContainerType = HudContainerType.Desktop,
        KeyboardHookBehaviour = Application.DesktopKeyboardHook,
        MouseHookBehaviour = Application.DesktopMouseHook,

        // TODO: retrieve optimal contiguous desktop region that can be captured by a desktop video provider
        VirtualBounds = SystemInformation.VirtualScreen
      });

      // create component managers for each container
      this.containers = this.containers.Select(c => {
        c.TidbitManager = new TidbitManager(c);
        return c;
      }).ToList();
    }

    /// <inheritdoc />
    /// <summary>
    ///   Releases resources
    /// </summary>
    public void Dispose() => this.containers.ForEach(c => {
      c.KeyboardHookBehaviour?.RequestUnlock();
      c.MouseHookBehaviour?.RequestUnlock();
    });

    /// <summary>
    ///   Gets the current HUD container
    /// </summary>
    /// <returns>A structure with HUD container information</returns>
    internal HudContainerInfo GetContainer() => this.containers.First();
  }
}