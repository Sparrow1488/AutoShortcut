using Sparrow.Video.Abstractions.Pipelines;
using Sparrow.Video.Abstractions.Pipelines.Options;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Shortcuts.Projects;

namespace Sparrow.Video.Shortcuts.Pipelines
{
    public class ShortcutPipeline : IShortcutPipeline
    {
        public ShortcutPipeline(IPipelineOptions options)
        {
            _options = options;
        }

        private readonly IPipelineOptions _options;
        internal ICollection<IProjectFile> ProjectFiles { get; set; } = new List<IProjectFile>();

        public IPipeline Configure(Action<IPipelineOptions> options)
        {
            options.Invoke(_options);
            return this;
        }

        public IProject CreateProject()
        {
            ApplyRules();
            return new ShortcutProject()
            {
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
    }
}
