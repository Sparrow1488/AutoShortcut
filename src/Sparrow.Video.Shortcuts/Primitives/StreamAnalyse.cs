using Newtonsoft.Json;
using Sparrow.Video.Abstractions.Primitives;
using System.Text.Json;

namespace Sparrow.Video.Shortcuts.Primitives;

public class StreamAnalyse : IStreamAnalyse
{
    [JsonProperty("index")]
    public int Index { get; set; }
    [JsonProperty("codec_name")] 
    public string CodecName { get; set; }
    [JsonProperty("codec_type")] 
    public string CodecType { get; set; }
    [JsonProperty("duration")] 
    public double Duration { get; set; }
    [JsonProperty("bit_rate")]
    public int BitRate { get; set; }
}