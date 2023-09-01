using Newtonsoft.Json;
using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Shortcuts.Primitives;

public class FileFormat : IFileFormat
{
    [JsonProperty("format_name")]
    public string FormatName { get; set; } = string.Empty;
    public static readonly IFileFormat Default = new FileFormat();
}