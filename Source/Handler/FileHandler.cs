using System;
using System.Collections.Generic;
using System.IO;
using Aperture;
using Captain.Common;
using static Captain.Application.Application;

namespace Captain.Application {
  /// <inheritdoc cref="Handler" />
  /// <summary>
  ///   Saves captures to the filesystem
  /// </summary>
  [Common.DisplayName("Save to Disk")]
  [DisplayIcon(typeof(Resources), nameof(Resources.FileHandlerExtensionIcon))]
  internal class FileHandler : Handler, IStreamWrapper, IUriProvider {
    /// <summary>
    ///   Custom substitution templates
    /// </summary>
    private readonly Dictionary<CommonVariable, Func<object>> templates = TemplateHelper.Templates;

    /// <summary>
    ///   Local file name
    /// </summary>
    private readonly string fileName;

    /// <inheritdoc />
    /// <summary>
    ///   Class constructor
    /// </summary>
    /// <param name="workflow">Workflow originating the capture</param>
    /// <param name="codec">Capture codec</param>
    /// <param name="stream">Capture stream</param>
    /// <param name="options">Optional user-provided options</param>
    public FileHandler(WorkflowBase workflow, Codec codec, Stream stream, object options) : base(
      workflow, codec, stream, options) {
      string fileExtension = "";

      if (codec.GetMediaType() is MediaType mediaType) {
        fileExtension = mediaType.Extension;
      } else {
        Log.Warn("codec does not provide file extension information");
      }

      // add substitution templates
      this.templates[CommonVariable.Extension] = () => fileExtension;
      this.templates[CommonVariable.Type] = () => workflow.Type == WorkflowType.Still
        ? Resources.TemplateHelper_Type_Screenshot
        : Resources.TemplateHelper_Type_Recording;

      // construct file name from template
      string template = Resources.SaveToFile_DefaultNameTemplate;
      if (options is Dictionary<string, string> optionDict && optionDict.ContainsKey("PathTemplate")) {
        template = optionDict["PathTemplate"];
      }

      // open stream
      this.fileName = TemplateHelper.GetString(TemplateHelper.Normalize(template));
      Log.Info("opening output file stream: " + this.fileName);
      if (Path.GetDirectoryName(this.fileName) is string dirName) {
        Directory.CreateDirectory(dirName);
      }

      OutputStream = new FileStream(Environment.ExpandEnvironmentVariables(this.fileName), FileMode.CreateNew);
    }

    /// <inheritdoc />
    /// <summary>
    ///   Output file stream
    /// </summary>
    public Stream OutputStream { get; }

    /// <inheritdoc />
    /// <summary>
    ///   Gets and Uniform Resource Identifier (URI) for the handled capture
    /// </summary>
    /// <returns>An URI representing the captured object</returns>
    public Uri GetUri() => new Uri(this.fileName, UriKind.Absolute);

    /// <inheritdoc />
    /// <summary>
    ///   Handles captures
    /// </summary>
    public override void Handle() {
      if (OutputStream.Length == 0) {
        Log.Warn("encoded wrote no output - deleting created file");
        OutputStream.Dispose();

        try {
          File.Delete(this.fileName);
        } catch (Exception exception) {
          Log.Warn($"could not delete file: {exception}");
        }
      }

      OutputStream.Dispose();
    }
  }
}