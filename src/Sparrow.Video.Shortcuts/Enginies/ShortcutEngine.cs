using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sparrow.Video.Abstractions.Enginies;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Pipelines;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Processes.Settings;
using Sparrow.Video.Shortcuts.Render;
using Sparrow.Video.Shortcuts.Services.Options;

namespace Sparrow.Video.Shortcuts.Enginies;

public class ShortcutEngine : IShortcutEngine
{
    public ShortcutEngine(
        ILogger<ShortcutEngine> logger,
        IUploadFilesService uploadFilesService,
        IProjectFileCreator projectFileCreator,
        IRestoreProjectService restoreService,
        ITextFormatter textFormatter,
        IServiceProvider services,
        IPathsProvider pathsProvider,
        IRenderUtility renderUtility)
    {
        _logger = logger;
        _uploadFilesService = uploadFilesService;
        _projectFileCreator = projectFileCreator;
        _restoreService = restoreService;
        _textFormatter = textFormatter;
        _services = services;
        _pathsProvider = pathsProvider;
        _renderUtility = renderUtility;
    }

    private readonly ILogger<ShortcutEngine> _logger;
    private readonly IUploadFilesService _uploadFilesService;
    private readonly IProjectFileCreator _projectFileCreator;
    private readonly IRestoreProjectService _restoreService;
    private readonly ITextFormatter _textFormatter;
    private readonly IServiceProvider _services;
    private readonly IPathsProvider _pathsProvider;
    private readonly IRenderUtility _renderUtility;

    public async Task<IShortcutPipeline> CreatePipelineAsync(
        string filesDirectory, CancellationToken cancellationToken = default)
    {
        var uploadFilesSettings = new UploadFilesOptions()
        {
            OnUploadedIgnoreFile = file => UploadFileAction.Skip
        }.Ignore(FileType.Restore).Ignore(FileType.Undefined);

        var files = await _uploadFilesService.GetFilesAsync(filesDirectory, uploadFilesSettings, cancellationToken);
        _logger.LogInformation("Starting analyse files");
        var projectFilesList = new List<IProjectFile>();
        for (int i = 0; i < files.Count; i++)
        {
            IFile file = files.ElementAt(i);
            _logger.LogInformation($"({i+1}/{files.Count}) Analyse \"{_textFormatter.GetPrintable(file.Name)}\"");
            var projectFile = await _projectFileCreator.CreateAsync(file, cancellationToken);
            projectFilesList.Add(projectFile);
        }
        return CreateShortcutPipelineWithFiles(projectFilesList);
    }

    private IShortcutPipeline CreateShortcutPipelineWithFiles(
        IEnumerable<IProjectFile> files)
    {
        var pipeline = _services.GetRequiredService<IShortcutPipeline>();
        pipeline.SetFiles(files);
        return pipeline;
    }

    public async Task<IFile> StartRenderAsync(
        IProject project, CancellationToken cancellationToken = default)
    {
        var resultSaveSettings = new SaveSettings() {
            SaveFullPath = Path.Combine(
                _pathsProvider.GetPathFromCurrent("ResultFiles"), $"{project.Name}.mp4")
        };
        return await _renderUtility.StartRenderAsync(project, resultSaveSettings, cancellationToken);
    }

    public async Task<IFile> ContinueRenderAsync(
        string restoreDirectoryPath, CancellationToken cancellationToken = default)
    {
        var project = await _restoreService.RestoreAsync(restoreDirectoryPath, cancellationToken);
        return await StartRenderAsync(project, cancellationToken);
    }
}
