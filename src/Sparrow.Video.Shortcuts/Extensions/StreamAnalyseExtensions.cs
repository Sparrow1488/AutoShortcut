using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Shortcuts.Extensions;

public static class StreamAnalyseExtensions
{
    public static IVideoStreamAnalyse Video(this IEnumerable<IStreamAnalyse> streamAnalyses)
    {
        return (IVideoStreamAnalyse)streamAnalyses.First(x => x.CodecType.ToLower() == "video");
    }

    public static bool WithAudio(this IEnumerable<IStreamAnalyse> streamAnalyses)
    {
        return streamAnalyses.Any(x => x.CodecType.ToLower() == "audio");
    }
}