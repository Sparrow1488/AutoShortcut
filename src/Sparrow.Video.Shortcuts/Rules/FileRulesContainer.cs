using Newtonsoft.Json;
using Sparrow.Video.Abstractions.Pipelines.Options;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Rules;
using Sparrow.Video.Shortcuts.Exceptions;
using Sparrow.Video.Shortcuts.Factories;

namespace Sparrow.Video.Shortcuts.Rules;

[Serializable]
public class FileRulesContainer : IFileRulesContainer
{
    [JsonProperty]
    private readonly List<IFileRule> fileRules = new();

    public FileRulesContainer() { }

    [JsonConstructor]
    protected FileRulesContainer(
        List<IFileRule> fileRules)
    {
        this.fileRules = fileRules;
    }

    [JsonIgnore]
    public IEnumerable<IProcessingRule> ProcessingRules => fileRules;

    public void AddRule(IFileRule rule)
    {
        fileRules.Add(CreateFileRule(new(rule)));
    }

    public void AddRule<TRule>() 
        where TRule : IFileRule
    {
        fileRules.Add(CreateFileRule(new(typeof(TRule))));
    }

    private static IFileRule CreateFileRule(RuleStore storedRule)
    {
        IFileRule fileRule = null;
        if (storedRule.Kind is RuleStoreKind.Type)
            fileRule = FileRuleFactory.CreateDefaultRule(storedRule.RuleType);
        if (storedRule.Kind is RuleStoreKind.Instance)
            fileRule = storedRule.Instance.Clone();
        if (storedRule.Kind is RuleStoreKind.Null)
            throw new NotSpecifiedStoredRuleException("Hmm.. Something went wrong");
        return fileRule;
    }

    public void ApplyRules(IEnumerable<IProjectFile> projectFiles)
    {
        foreach (var file in projectFiles)
            foreach (var rule in ProcessingRules)
                if (rule.IsInRule(file))
                    file.RulesCollection.Add((IFileRule)rule.Clone());
    }
}
