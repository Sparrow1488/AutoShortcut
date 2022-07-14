using Sparrow.Video.Abstractions.Pipelines;
using Sparrow.Video.Abstractions.Pipelines.Options;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Projects.Options;
using Sparrow.Video.Abstractions.Rules;
using Sparrow.Video.Shortcuts.Primitives.Structures;

namespace Sparrow.Video.Shortcuts.Pipelines
{
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
        {
            return CreateProject(opt => opt.StructureBy(new NameStructure()));
        }

        public IProject CreateProject(Action<IProjectOptions> options)
        {
            ApplyRules();
            return _projectCreator.CreateProject(ProjectFiles, options);
        }

        //private void ApplyRules()
        //{
        //    foreach (var file in ProjectFiles)
        //        foreach (var rule in _options.Rules)
        //            if (rule.IsInRule(file))
        //                file.RulesCollection.Add(rule);
        //}

        private void ApplyRules()
        {
            foreach (var file in ProjectFiles)
                foreach (var ruleType in _options.RulesTypes)
                {
                    var rule = (IFileRule)Activator.CreateInstance(ruleType);
                    if (rule.IsInRule(file))
                    {
                        file.RulesCollection.Add(rule); // TODO: RulesFactory
                    }
                }
        }

        public IShortcutPipeline SetFiles(IEnumerable<IProjectFile> files)
        {
            ProjectFiles = files.ToArray();
            return this;
        }
    }
}
