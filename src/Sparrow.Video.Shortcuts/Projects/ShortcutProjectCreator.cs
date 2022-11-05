using Microsoft.Extensions.DependencyInjection;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Projects.Options;

namespace Sparrow.Video.Shortcuts.Projects;

public class ShortcutProjectCreator : IProjectCreator
{
    private readonly IProjectOptions _projectOptions;
    private readonly IServiceProvider _services;

    public ShortcutProjectCreator(
        IProjectOptions projectOptions,
        IServiceProvider services)
    {
        _projectOptions = projectOptions;
        _services = services;
    }

    public IProject CreateProject(IEnumerable<IProjectFile> files)
    {
        return CreateProject(files, options => options.StructureBy(options.DefaultStructure));
    }

    public IProject CreateProject(IEnumerable<IProjectFile> files, Action<IProjectOptions> options)
    {
        options?.Invoke(_projectOptions);
        return CreateProjectWithOptions(files, _projectOptions);
    }

    public IProject CreateProjectWithOptions(IEnumerable<IProjectFile> files, IProjectOptions options)
    {
        var emptyProject = CreateEmptyProject(options);
        emptyProject.Files = emptyProject.Options.Structure.GetStructuredFiles(files).ToArray();
        return emptyProject;
    }

    private ShortcutProject CreateEmptyProject(IProjectOptions options = default)
    {
        var project = ActivatorUtilities.CreateInstance<ShortcutProject>(_services);
        project.Options = options ?? project.Options;
        return project;
    }
}
