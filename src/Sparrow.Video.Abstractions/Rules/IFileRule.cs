using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Abstractions.Rules
{
    public interface IFileRule
    {
        bool IsApplied { get; }
        RuleName RuleName { get; }
        bool IsInRule(IProjectFile file);
        void Applied();
    }
}
