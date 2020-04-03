using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Captain.Common;
using Newtonsoft.Json;
using static Captain.Application.Application;

// ReSharper disable MemberCanBeInternal
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace Captain.Application {
  /// <summary>
  ///   Class representing application options.
  /// </summary>
  /// <remarks>
  ///   Instances of this class will be serialized and written to the options file. Make sure to keep this class
  ///   backwards-compatible! A single change may render the application unresponsive when upgrading to a different
  ///   version.
  /// </remarks>
  [Serializable]
  public sealed class Options {
    /// <summary>
    ///   Default options file name
    /// </summary>
    internal const string OptionsFileName = "Options.json";

    /// <summary>
    ///   Saved position for windows
    /// </summary>
    public Dictionary<string, Point> WindowPositions { get; set; } = new Dictionary<string, Point>();

    /// <summary>
    ///   Current Options dialog tab index
    /// </summary>
    public uint OptionsDialogTab { get; set; }

    /// <summary>
    ///   Display status popups
    /// </summary>
    public bool EnableStatusPopups { get; set; } = true;

    /// <summary>
    ///   Adjusts the behavior of the update manager
    /// </summary>
    public UpdatePolicy UpdatePolicy { get; set; } = UpdatePolicy.CheckOnly;

    /// <summary>
    ///   Last version of the application that was ran
    /// </summary>
    public string LastVersion { get; set; } = String.Empty;

    /// <summary>
    ///   Enumerates the user-defined workflows
    /// </summary>
    public List<WorkflowBase> Workflows { get; set; } = new List<WorkflowBase>();

    /// <summary>
    ///   Loads an <see cref="Options" /> instance from file
    /// </summary>
    /// <returns>An instance of the <see cref="Options" /> class</returns>
    internal static Options Load() {
      try {
        using (var fileStream = new FileStream(Path.Combine(Application.FsManager.GetSafePath(), OptionsFileName),
          FileMode.OpenOrCreate)) {
          if (fileStream.Length == 0) {
            Log.Warn("stream is empty");
            return null;
          }

          using (var reader = new StreamReader(fileStream)) {
            return JsonConvert.DeserializeObject<Options>(reader.ReadToEnd(), new JsonSerializerSettings());
          }
        }
      } catch (Exception exception) { Log.Warn($"could not load options - {exception}"); }
      return null;
    }

    /// <summary>
    ///   Saves these options to the stream they were loaded from
    /// </summary>
    internal void Save() {
      Log.Trace("saving options");

#if !DEBUG
      try {
#endif
      using (var fileStream = new FileStream(Path.Combine(Application.FsManager.GetSafePath(), OptionsFileName),
        FileMode.OpenOrCreate)) {
        fileStream.SetLength(0);

        using (var writer = new StreamWriter(fileStream)) {
          writer.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
        }
      }
#if !DEBUG
      } catch (Exception exception) {
        Log.Warn($"could not save options - {exception}");
      }
#endif
    }
  }
}