using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Rules;

namespace Sparrow.Console.Rules
{
    public class LoopFileRule : FileRuleBase
    {
        public int LoopCount => 2;
        public override Func<IProjectFile, bool> Condition => file => file.Analyse.StreamAnalyses.Video().Duration <= 8;
        public override RuleApply RuleApply => RuleApply.Runtime;

        public override RuleName RuleName { get; } = RuleName.New("Loop");
    }
}
