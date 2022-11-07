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
    private List<IFileRule> fileRules = new();

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

    private IFileRule CreateFileRule(RuleStore storedRule)
    {
        IFileRule fileRule = null;
        if (storedRule.Kind is RuleStoreKind.Type)
            fileRule = FileRuleFactory.CreateDefaultRule(storedRule.RuleType);
        if (storedRule.Kind is RuleStoreKind.Instance)
            fileRule = storedRule.Instance.Clone();
        if (storedRule.Kind is RuleStoreKind.Null)
            throw new NotSpecifiedStoredRuleException("Hmm.. Something went wrong");
        if (Contains(fileRule.GetType()))
            throw new RuleAlreadyExistsException($"{fileRule.GetType().Name} already exists in container");
        return fileRule;
    }

    public void ApplyRules(IEnumerable<IProjectFile> projectFiles)
    {
        foreach (var file in projectFiles)
            foreach (var rule in ProcessingRules)
                if (rule.IsInRule(file))
                    file.RulesCollection.Add((IFileRule)rule.Clone());
    }

    public void ApplyRules(IProjectFile projectFile)
    {
        throw new NotImplementedException();
    }

    public void DeleteRule<TRule>() 
        where TRule : IFileRule
    {
        if (Contains<TRule>())
        {
            var containable = GetRule<TRule>();
            fileRules.Remove(containable);
        }
    }

    public bool Contains<TRule>() 
        where TRule : IFileRule
    {
        return fileRules.Any(x => x.GetType() == typeof(TRule));
    }

    public bool Contains(Type ruleType)
    {
        return fileRules.Any(x => x.GetType() == ruleType);
    }

    public void Replace<TRule>(TRule @new) 
        where TRule : IFileRule
    {
        var replacedFileRules = new List<IFileRule>();
        foreach (var rule in fileRules)
        {
            if (rule.GetType() == typeof(TRule))
                replacedFileRules.Add(@new);
            else replacedFileRules.Add(rule);
        }
        fileRules = replacedFileRules;
    }

    public TRule GetRule<TRule>() 
        where TRule : IFileRule
    {
        return (TRule)fileRules.Single(x => x.GetType() == typeof(TRule));
    }
}
