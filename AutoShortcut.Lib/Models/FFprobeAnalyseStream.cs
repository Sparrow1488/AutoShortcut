using AutoShortcut.Lib.Contracts.Enums;
using Newtonsoft.Json;

namespace AutoShortcut.Lib.Models;

/// <summary>
/// Анализ файла утилитой ffprobe
/// </summary>
public class FFprobeAnalyseStream
{
    [JsonProperty("index")]
    public byte Index { get; set; }
    [JsonProperty("codec_name")] 
    public string CodecName { get; set; } = string.Empty;
    [JsonProperty("width")]
    public int Width { get; set; }
    [JsonProperty("height")]
    public int Height { get; set; }
    [JsonProperty("duration")]
    public double Duration { get; set; }
    [JsonProperty("bit_rate")]
    public int BitRate { get; set; }
    [JsonProperty("display_aspect_ratio")]
    public string? DisplayAspectRation { get; set; }
    [JsonProperty("codec_type")]
    public CodecType CodecType { get; set; }
}