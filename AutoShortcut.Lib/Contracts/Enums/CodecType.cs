using Newtonsoft.Json;

namespace AutoShortcut.Lib.Contracts.Enums;

public enum CodecType
{
    [JsonProperty("video")]
    Video = 0,
    [JsonProperty("audio")]
    Audio = 1,
    [JsonProperty("data")]
    Data = 2
}