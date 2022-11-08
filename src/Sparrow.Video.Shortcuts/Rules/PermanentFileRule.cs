using Sparrow.Video.Abstractions.Enums;

namespace Sparrow.Video.Shortcuts.Rules;

public abstract class PermanentFileRule : FileRuleBase
{
    public override RuleApply RuleApply => RuleApply.Permanent;
}
