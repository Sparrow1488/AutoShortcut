using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Shortcuts.Extensions;

namespace Sparrow.Console.Rules
{
    public class ApplicationFileRules
    {
        public static readonly LoopFileRule LoopMediumFileRule = new(
            file => file.Analyse.StreamAnalyses.Video().Duration < 14 && 
                    file.Analyse.StreamAnalyses.Video().Duration > 8)
        {
            LoopCount = 2
        };
        public static readonly LoopFileRule LoopShortFileRule = new(
            file => file.Analyse.StreamAnalyses.Video().Duration <= 8)
        {
            LoopCount = 3
        };
        public static readonly FormatFileRule FormatFileRule = new(
            Resolution.HighFHD, file => true);
        public static readonly SilentFileRule SilentFileRule = new(
            file => !file.Analyse.StreamAnalyses.WithAudio());
        public static readonly EncodingFileRule EncodingFileRule = new(file => true)
        {
            EncodingType = EncodingType.Mpegts
        };
    }
}
