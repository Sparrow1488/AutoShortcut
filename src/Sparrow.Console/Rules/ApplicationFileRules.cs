using Sparrow.Video.Shortcuts.Extensions;

namespace Sparrow.Console.Rules
{
    public class ApplicationFileRules
    {
        public static readonly LoopFileRule LoopFileRule = new(
            file => file.Analyse.StreamAnalyses.Video().Duration < 13);
    }
}
