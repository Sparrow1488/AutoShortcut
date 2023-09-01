using Newtonsoft.Json;

namespace AutoShortcut.Lib.Models;

public class FFprobeFormat
{
    [JsonProperty("duration")]
    public double Duration { get; set; }
    /// <summary>
    /// Перечисление форматов через запятую (прим.: mov,mp4,m4a,3gp,3g2,mj2)
    /// </summary>
    [JsonProperty("format_name")]
    public string? FormatName { get; set; }
}