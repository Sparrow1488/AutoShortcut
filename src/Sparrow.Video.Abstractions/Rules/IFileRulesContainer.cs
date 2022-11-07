using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Abstractions.Rules;

public interface IFileRulesContainer
{
    void AddRule(IFileRule rule);
    void AddRule<TRule>() where TRule : IFileRule;
    void DeleteRule<TRule>() where TRule : IFileRule;
    bool Contains<TRule>() where TRule : IFileRule;
    void Replace<TRule>(TRule @new) where TRule : IFileRule;
    TRule GetRule<TRule>() where TRule : IFileRule;
    void ApplyRules(IEnumerable<IProjectFile> projectFiles);
}
