using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Aperture;
using Captain.Common;
using static Captain.Application.Application;
using FsManager = Captain.Common.FsManager;

namespace Captain.Application {
  /// <summary>
  ///   Manages extension resources
  /// </summary>
  internal sealed class ExtensionManager {
    /// <summary>
    ///   Contains a dictionary with common type names as keys and a list of actual types from loaded assemblies as
    ///   values
    /// </summary>
    private Dictionary<string, List<Type>> TypePool { get; } = new Dictionary<string, List<Type>> {
      {
        typeof(StillImageCodec).FullName, new List<Type> {
          typeof(PngWicStillImageCodec)
        }
      }, {
        typeof(VideoCodec).FullName, new List<Type> {
          typeof(HevcMediaFoundationVideoCodec)
        }
      }, {
        typeof(Handler).FullName, new List<Type> {
          typeof(ClipboardHandler),
          typeof(FileHandler)
        }
      }
    };

    /// <summary>
    ///   Class constructor.
    ///   Looks for installed extensions and registers them
    /// </summary>
    internal ExtensionManager() {
      // get all extension assemblies
      var assemblyLocations =
        Directory.EnumerateFiles(Application.FsManager.GetSafePath(FsManager.PluginPath)).ToList();

      // place plugin exports in type pool
      foreach (string path in assemblyLocations) {
        Log.Debug("loading assembly: " + path);

        Assembly asm;
        try {
          // load assembly
          asm = Assembly.LoadFile(path);
        } catch (Exception exception) {
          Log.Error("could not load assembly: " + exception);
          continue;
        }

        // assembly was successfully loaded, discover types
        int typeCount = 0;
        foreach (string commonType in TypePool.Keys) {
          Type[] types = asm.ExportedTypes.Where(t =>
                                                   t.IsClass &&
                                                   !t.IsAbstract &&
                                                   !t.ContainsGenericParameters &&
                                                   t.IsPublic &&
                                                   t.IsVisible &&
                                                   t.GetNestedType(commonType) != null).ToArray();
          TypePool[commonType].AddRange(types);
          typeCount = types.Length;
        }

        Log.Debug($"added {typeCount} type(s)");
      }
    }

    /// <summary>
    ///   Enumerates all loaded objects of the specified type
    /// </summary>
    /// <typeparam name="T">Type of the objects</typeparam>
    /// <returns>An enumeration of the loaded types</returns>
    internal List<Type> EnumerateObjects<T>() {
      return TypePool[typeof(T).FullName ?? throw new InvalidOperationException()].ToList();
    }

    /// <summary>
    ///   Creates an instance of a registered type
    /// </summary>
    /// <typeparam name="T">Must be the base type of the requested one</typeparam>
    /// <param name="typeName">Type of the object</param>
    /// <param name="constructorParams">Optional constructor parameters</param>
    /// <returns>An instance of the requested type</returns>
    /// <exception cref="TypeLoadException">Thrown when an unregistered type is requested</exception>
    internal T CreateObject<T>(string typeName, params object[] constructorParams) {
      Type type;

      try {
        type = TypePool[typeof(T).FullName ?? throw new ArgumentException(nameof(T))]
          .First(t => t.FullName == typeName);
      } catch (Exception exception) {
        Log.Warn($"could not find type {typeName}:{typeof(T).FullName}");
        throw new TypeLoadException("No such type has been registered", exception);
      }

      return (T) Activator.CreateInstance(type, constructorParams);
    }
  }
}