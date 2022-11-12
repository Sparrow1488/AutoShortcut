using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Abstractions.Rules;

public interface IFileRulesContainer : IEnumerable<IFileRule>
{
    void AddRule(IFileRule rule);
    void AddRule<TRule>() where TRule : IFileRule;
    void AddRuleAfter<TRule>(Type afterRule) where TRule : IFileRule;
    void DeleteRule<TRule>() where TRule : IFileRule;
    bool Contains<TRule>() where TRule : IFileRule;
    bool Contains(Type ruleType);
    void Replace<TRule>(TRule @new) where TRule : IFileRule;
    TRule GetRule<TRule>() where TRule : IFileRule;
    void ApplyRules(IEnumerable<IProjectFile> projectFiles);
    void ApplyRules(IProjectFile projectFile);
    IFileRulesContainer Merge(IFileRulesContainer container);
}
