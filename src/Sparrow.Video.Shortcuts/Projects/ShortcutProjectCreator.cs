using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Projects.Options;
using Sparrow.Video.Abstractions.Rules;
using Sparrow.Video.Shortcuts.Projects.Options;

namespace Sparrow.Video.Shortcuts.Projects;

public class ShortcutProjectCreator : IProjectCreator
{
    private readonly ProjectOptions _projectOptions;

    public ShortcutProjectCreator(ISharedProject sharedProject)
    {
        SharedProject = sharedProject;
        _projectOptions = ProjectOptions.Create();
    }

    public ISharedProject SharedProject { get; }

    public IProject CreateProject(IEnumerable<IProjectFile> files)
    {
        return CreateProject(files, options => options.StructureBy(ProjectOptions.DefaultStructure));
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
        var structuredFiles = options.Structure.GetStructuredFiles(files);
        var project = ShortcutProject.Create(options, structuredFiles);

        SetRulesToNewFiles(project.Options.RulesContainer, files);

        SharedProject.Project = project;

        return project;
    }

    private static void SetRulesToNewFiles(
        IFileRulesContainer rules, IEnumerable<IProjectFile> projectFiles)
    {
        foreach (var projectFile in projectFiles)
        {
            //var merged = rules.Merge(projectFile.RulesContainer);
            rules.ApplyRules(projectFile);
        }

        //var filesWithoutRules = projectFiles.Where(x => !x.RulesContainer.Any());
        var filesWithoutRules = projectFiles.Where(x => true).ToArray();
        rules.ApplyRules(filesWithoutRules);
    }
}
