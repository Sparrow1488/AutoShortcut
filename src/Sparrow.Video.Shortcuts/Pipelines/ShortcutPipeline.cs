using Sparrow.Video.Abstractions.Pipelines;
using Sparrow.Video.Abstractions.Pipelines.Options;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Projects.Options;
using Sparrow.Video.Shortcuts.Primitives.Structures;
using Sparrow.Video.Shortcuts.Projects;

namespace Sparrow.Video.Shortcuts.Pipelines
{
    public class ShortcutPipeline : IShortcutPipeline
    {
        public ShortcutPipeline(
            IProjectOptions projectOptions,
            IPipelineOptions options)
        {
            _projectOptions = projectOptions;
            _options = options;
        }

        private readonly IProjectOptions _projectOptions;
        private readonly IPipelineOptions _options;
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
            options.Invoke(_projectOptions);
            ProjectFiles = _projectOptions.Structure.GetStructuredFiles(ProjectFiles).ToArray();
            return new ShortcutProject() {
                Files = ProjectFiles
            };
        }

        private void ApplyRules()
        {
            foreach (var file in ProjectFiles)
                foreach (var rule in _options.Rules)
                    if (rule.IsInRule(file))
                        file.RulesCollection.Add(rule);
        }

        public IShortcutPipeline SetFiles(IEnumerable<IProjectFile> files)
        {
            ProjectFiles = files.ToArray();
            return this;
        }
    }
}
