using Microsoft.Extensions.Logging;
using Sparrow.Video.Abstractions.Enginies;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Projects.Options;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Processes.Settings;
using Sparrow.Video.Shortcuts.Projects.Options;
using Sparrow.Video.Shortcuts.Render;

namespace Sparrow.Video.Shortcuts.Enginies;

public class ShortcutEngine : IShortcutEngine
{
    private readonly ILogger<ShortcutEngine> _logger;
    private readonly IProjectFileCreator _projectFileCreator;
    private readonly IProjectCreator _projectCreator;
    private readonly ITextFormatter _textFormatter;
    private readonly IPathsProvider _pathsProvider;
    private readonly IRenderUtility _renderUtility;

    public ShortcutEngine(
        ILogger<ShortcutEngine> logger,
        IProjectFileCreator projectFileCreator,
        IProjectCreator projectCreator,
        ITextFormatter textFormatter,
        IPathsProvider pathsProvider,
        IRenderUtility renderUtility)
    {
        _logger = logger;
        _projectFileCreator = projectFileCreator;
        _projectCreator = projectCreator;
        _textFormatter = textFormatter;
        _pathsProvider = pathsProvider;
        _renderUtility = renderUtility;
    }

    public async Task<IProject> CreateProjectAsync(
        Action<IProjectOptions> options, 
        IEnumerable<IFile> files,
        CancellationToken cancellationToken = default)
    {
        var projectOptions = ProjectOptions.Create();

        var filesArray = files.ToArray();
        var projectFiles = new List<IProjectFile>();

        _logger.LogInformation("Starting create project");

        for (int i = 0; i < filesArray.Length; i++)
        {
            var file = filesArray[i];

            _logger.LogInformation($"({i + 1}/{filesArray.Length}) Creating file \"{_textFormatter.GetPrintable(file.Name)}\"");

            var projectFile = await _projectFileCreator.CreateAsync(file, cancellationToken);
            projectFiles.Add(projectFile);
        }

        var project = _projectCreator.CreateProject(projectFiles, options);
        return project;
    }

    public async Task<IFile> StartRenderAsync(
        IProject project, CancellationToken cancellationToken = default)
    {
        var resultSaveSettings = new SaveSettings() {
            SaveFullPath = Path.Combine(
                _pathsProvider.GetPathFromSharedProject("ResultFiles"), $"{project.Options.ProjectName}.mp4")
        };
        return await _renderUtility.StartRenderAsync(project, resultSaveSettings, cancellationToken);
    }
}
