using Newtonsoft.Json;

namespace AutoShortcut.Lib.Models;

public class FFprobeAnalyseData
{
    [JsonProperty("streams")]
    public IEnumerable<FFprobeAnalyseStream> Streams { get; set; } = new List<FFprobeAnalyseStream>();
    [JsonProperty("format")]
    public FFprobeFormat? Format { get; set; }
}