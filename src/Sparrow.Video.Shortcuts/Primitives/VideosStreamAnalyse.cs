using Newtonsoft.Json;
using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Shortcuts.Primitives
{
    public class VideosStreamAnalyse : StreamAnalyse, IVideoStreamAnalyse
    {
        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("height")]
        public int Height { get; set; }
        [JsonProperty("display_aspect_ratio")]
        public string DisplayAspectRatio { get; set; }
    }
}
