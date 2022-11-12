using Microsoft.Extensions.DependencyInjection;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Projects.Options;

namespace Sparrow.Video.Shortcuts.Projects
{
    public class ShortcutProjectCreator : IProjectCreator
    {
        public ShortcutProjectCreator(
            IProjectOptions projectOptions,
            IServiceProvider services)
        {
            _projectOptions = projectOptions;
            _services = services;
        }

        private readonly IProjectOptions _projectOptions; // TODO: Factory
        private readonly IServiceProvider _services;

        public IProject CreateProjectWithOptions(IEnumerable<IProjectFile> files, IProjectOptions options)
        {
            var emptyProject = CreateEmptyProject(options);
            emptyProject.Files = emptyProject.Options.Structure.GetStructuredFiles(files).ToArray();
            return emptyProject;
        }

        public IProject CreateProject(IEnumerable<IProjectFile> files, Action<IProjectOptions> options)
        {
            options.Invoke(_projectOptions);
            return CreateProject(files);
        }

        public IProject CreateProject(IEnumerable<IProjectFile> files)
        {
            var project = CreateEmptyProject();
            project.Files = files.ToArray();
            return project;
        }

        private ShortcutProject CreateEmptyProject(IProjectOptions options = default)
        {
            var project = ActivatorUtilities.CreateInstance<ShortcutProject>(_services);
            project.Options = options ?? project.Options;
            return project;
        }
    }
}
