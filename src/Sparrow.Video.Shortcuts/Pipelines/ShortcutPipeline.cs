using Sparrow.Video.Abstractions.Pipelines;
using Sparrow.Video.Abstractions.Pipelines.Options;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Projects.Options;
using Sparrow.Video.Abstractions.Rules;
using Sparrow.Video.Shortcuts.Exceptions;
using Sparrow.Video.Shortcuts.Factories;
using Sparrow.Video.Shortcuts.Primitives.Structures;

namespace Sparrow.Video.Shortcuts.Pipelines;

public class ShortcutPipeline : IShortcutPipeline
{
    public ShortcutPipeline(
        IPipelineOptions options,
        IProjectCreator projectCreator)
    {
        _options = options;
        _projectCreator = projectCreator;
    }

    private readonly IPipelineOptions _options;
    private readonly IProjectCreator _projectCreator;

    public ICollection<IProjectFile> ProjectFiles { get; set; } = new List<IProjectFile>();

    public IPipeline Configure(Action<IPipelineOptions> options)
    {
        options.Invoke(_options);
        return this;
    }

    public IProject CreateProject()
        => CreateProject(opt => opt.StructureBy(new NameStructure()));

    public IProject CreateProject(Action<IProjectOptions> options)
    {
        ApplyRules();
        return _projectCreator.CreateProject(ProjectFiles, options);
    }

    private void ApplyRules()
    {
        foreach (var file in ProjectFiles)
            foreach (var storedRule in _options.Stores)
            {
                IFileRule fileRule = null;
                if (storedRule.Kind is RuleStoreKind.Type)
                    fileRule = FileRuleFactory.CreateDefaultRule(storedRule.RuleType);
                if (storedRule.Kind is RuleStoreKind.Instance)
                    fileRule = storedRule.Instance.Clone();
                if (storedRule.Kind is RuleStoreKind.Null)
                    throw new NotSpecifiedStoredRuleException("Hmm.. Something went wrong");

                if (fileRule.IsInRule(file))
                    file.RulesCollection.Add(fileRule);
            }
    }

    public IShortcutPipeline SetFiles(IEnumerable<IProjectFile> files)
    {
        ProjectFiles = files.ToArray();
        return this;
    }
}
