using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Abstractions.Rules;

public interface IFileRulesContainer
{
    void AddRule(IFileRule rule);
    void AddRule<TRule>() where TRule : IFileRule;
    void ApplyRules(IEnumerable<IProjectFile> projectFiles);
}
