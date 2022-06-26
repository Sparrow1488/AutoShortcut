using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Abstractions.Rules
{
    public interface IFileRule
    {
        RuleName RuleName { get; }
        bool IsInRule(IProjectFile file);
    }
}
