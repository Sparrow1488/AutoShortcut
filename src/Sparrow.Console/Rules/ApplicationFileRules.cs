using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Processes.Settings;

namespace Sparrow.Console.Rules
{
    public class ApplicationFileRules
    {
        public static readonly LoopFileRule LoopFileRule = new(file => true)
        {
            LoopCount = 2
        };
        public static readonly FormatFileRule FormatFileRule = new(file => true);
        public static readonly SilentFileRule SilentFileRule = new(
            file => !file.Analyse.StreamAnalyses.WithAudio());
        public static readonly EncodingFileRule EncodingFileRule = new(file => true)
        {
            EncodingType = EncodingType.Mpegts
        };
    }
}
