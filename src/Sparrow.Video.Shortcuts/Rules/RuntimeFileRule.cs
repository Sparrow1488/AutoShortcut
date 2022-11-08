using Sparrow.Video.Abstractions.Enums;

namespace Sparrow.Video.Shortcuts.Rules;

public abstract class RuntimeFileRule : FileRuleBase
{
    public override RuleApply RuleApply => RuleApply.Runtime;
}
