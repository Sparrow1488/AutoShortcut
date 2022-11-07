using Newtonsoft.Json;
using Sparrow.Video.Abstractions.Pipelines.Options;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Rules;
using Sparrow.Video.Shortcuts.Exceptions;
using Sparrow.Video.Shortcuts.Factories;
using System.Collections;

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
                    file.RulesContainer.AddRule((IFileRule)rule.Clone());
    }

    public void ApplyRules(IProjectFile projectFile)
    {
        var newProjectRules = new FileRulesContainer();
        foreach (var rule in projectFile.RulesContainer)
        {
            var ruleType = rule.GetType();
            if (Contains(ruleType))
            {
                var currentRule = GetRule(ruleType);
                if (!currentRule.Equals(rule))
                {
                    newProjectRules.AddRule(currentRule.Clone());
                }
                else
                {
                    newProjectRules.AddRule(rule);
                }
            }
        }
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
        for (int i = 0; i < fileRules.Count; i++)
        {
            var currentRule = fileRules[i];
            if (currentRule.GetType() == typeof(TRule))
                fileRules[i] = @new;
            else fileRules[i] = currentRule;
        }
    }

    public TRule GetRule<TRule>() 
        where TRule : IFileRule
    {
        return (TRule)fileRules.Single(x => x.GetType() == typeof(TRule));
    }

    public IFileRule GetRule(Type type)
    {
        return fileRules.Single(x => x.GetType() == type);
    }

    public IEnumerator<IFileRule> GetEnumerator() => fileRules.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IFileRulesContainer Merge(IFileRulesContainer container)
    {
        // Сливаем полученный с текущим
        // 1. Одинаковая длинна (пока что)
        // 2. 
        // 3. 
        var rulesArray = container.ToArray();
        var selectedRules = fileRules.Where(x => container.Contains(x.GetType())).ToArray();
        var merged = new FileRulesContainer();
        for (int i = 0; i < rulesArray.Length; i++)
        {
            var currentRule = selectedRules[i];
            var outsideRule = rulesArray[i];

            if (currentRule.GetType() != outsideRule.GetType())
                throw new Exception("Merge failed: different types on same index");

            if (!currentRule.Equals(outsideRule))
                merged.AddRule(currentRule.Clone());
            else merged.AddRule(outsideRule.Clone());
        }
        return merged;
    }
}
