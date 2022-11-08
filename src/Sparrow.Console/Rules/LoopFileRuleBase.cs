using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Shortcuts.Rules;

namespace Sparrow.Console.Rules;

public abstract class LoopFileRuleBase : RuntimeFileRule
{
    public abstract int LoopCount { get; }
    public override RuleName RuleName => RuleName.Loop;
}
