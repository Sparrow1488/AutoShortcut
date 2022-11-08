using Newtonsoft.Json;
using Sparrow.Video.Abstractions.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Processes.Settings;

[Serializable]
public class EncodingSettings : IEncodingSettings
{
    [JsonProperty]
    public string EncodingType { get; set; }

    public static EncodingSettings Create(string encodingType)
    {
        return new()
        {
            EncodingType = encodingType
        };
    }
}
