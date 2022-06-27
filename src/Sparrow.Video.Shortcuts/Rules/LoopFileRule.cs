using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Shortcuts.Extensions;

namespace Sparrow.Video.Shortcuts.Rules
{
    public class LoopFileRule : FileRuleBase
    {
        public LoopFileRule(int loopCount, Func<IProjectFile, bool> condition) : base(condition)
        {
            LoopCount = loopCount;
        }

        public override RuleName RuleName => RuleName.Loop;
        public int LoopCount { get; }
        public static LoopFileRule Default = 
            new LoopFileRule(2, file => file.Analyse.StreamAnalyses.Video().Duration < 12);
    }
}