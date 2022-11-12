using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Abstractions.Rules;

public interface IProcessingRule : ICloneable
{
    RuleName RuleName { get; }
    RuleApply RuleApply { get; }
    bool IsInRule(IProjectFile file);
}