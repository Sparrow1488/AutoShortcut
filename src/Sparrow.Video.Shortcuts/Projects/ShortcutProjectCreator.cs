using Microsoft.Extensions.DependencyInjection;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Projects.Options;
using Sparrow.Video.Abstractions.Rules;
using Sparrow.Video.Shortcuts.Projects.Options;

namespace Sparrow.Video.Shortcuts.Projects;

public class ShortcutProjectCreator : IProjectCreator
{
    private readonly ProjectOptions _projectOptions;
    private readonly IServiceProvider _services;

    public ShortcutProjectCreator(
        IServiceProvider services,
        ISharedProject sharedProject)
    {
        _projectOptions = ProjectOptions.Create();
        _services = services;
        SharedProject = sharedProject;
    }

    public ISharedProject SharedProject { get; }

    public IProject CreateProject(IEnumerable<IProjectFile> files)
    {
        return CreateProject(files, options => options.StructureBy(options.DefaultStructure));
    }

    public IProject CreateProject(IEnumerable<IProjectFile> files, Action<IProjectOptions> options)
    {
        _projectOptions.ProjectFilesPaths = files.Select(x => x.File.Path).ToArray();
        options?.Invoke(_projectOptions);
        return CreateProjectWithOptions(files, _projectOptions);
    }

    public IProject CreateProjectWithOptions(
        IEnumerable<IProjectFile> files, 
        IProjectOptions options)
    {
        var project = CreateEmptyProject(options);
        project.Files = project.Options.Structure.GetStructuredFiles(files).ToArray();

        SetRulesToNewFiles(project.Options.RulesContainer, files);

        SharedProject.Project = project;

        return project;
    }

    private ShortcutProject CreateEmptyProject(
        IProjectOptions options = default)
    {
        var project = ActivatorUtilities.CreateInstance<ShortcutProject>(_services);
        project.Options = options ?? project.Options;
        return project;
    }

    private void SetRulesToNewFiles(
        IFileRulesContainer rules, IEnumerable<IProjectFile> projectFiles)
    {
        var filesWithoutRules = projectFiles.Where(x => !x.RulesCollection.Any());
        rules.ApplyRules(filesWithoutRules);
    }
}
