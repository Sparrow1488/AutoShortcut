using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Shortcuts.Rules;

namespace Sparrow.Console.Rules
{
    public abstract class LoopFileRuleBase : FileRuleBase
    {
        public virtual int LoopCount => 2;
        public override RuleApply RuleApply => RuleApply.Runtime;

        public override RuleName RuleName { get; } = RuleName.New("Loop");
    }
}
