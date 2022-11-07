using Microsoft.Extensions.Logging;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Primitives;

namespace Sparrow.Video.Shortcuts.Services;

public class RestoreProjectService : IRestoreProjectService
{
    private readonly ILogger<RestoreProjectService> _logger;
    private readonly IRestoreProjectOptionsService _restoreOptionsService;
    private readonly IRestoreFilesService _restoreFilesService;
    private readonly IProjectCreator _creator;
    private readonly IPathsProvider _pathsProvider;

    public RestoreProjectService(
        IProjectCreator creator,
        IPathsProvider pathsProvider,
        ILogger<RestoreProjectService> logger,
        IRestoreProjectOptionsService restoreOptionsService,
        IRestoreFilesService restoreFilesService)
    {
        _logger = logger;
        _restoreOptionsService = restoreOptionsService;
        _restoreFilesService = restoreFilesService;
        _creator = creator;
        _pathsProvider = pathsProvider;
    }

    public async Task<IProject> RestoreAsync(
        string filesDirectoryPath, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
        //_logger.LogInformation("Starting restore project");
        //var restoredFiles = await _restoreFilesService.RestoreFilesAsync(filesDirectoryPath);
        //var restoredProjectOptions = await _restoreOptionsService.RestoreOptionsAsync();

        //var restoredProject = _creator.CreateProjectWithOptions(
        //                            files: restoredFiles.Select(x => x.RestoredProjectFile), 
        //                            options: restoredProjectOptions);

        //_logger.LogInformation("Project restored");
        //return restoredProject;
    }

    public async Task<IProject> RestoreExistsAsync(
        string projectPath, CancellationToken cancellationToken = default)
    {
        StringPath.CreateExists(projectPath);
        _logger.LogInformation("Starting restore project");

        var optionsAbsolutePath = _pathsProvider.GetPath("ProjectOptions");
        var optionsFullPath = Path.Combine(projectPath, optionsAbsolutePath);

        var projectOptions = await _restoreOptionsService.RestoreOptionsAsync(optionsFullPath);
        var files = await _restoreFilesService.RestoreFilesAsync(projectOptions.ProjectFilesPaths);

        var project = _creator.CreateProjectWithOptions(
                                    files: files.Select(x => x.RestoredProjectFile),
                                    options: projectOptions);

        _logger.LogInformation("Project restored");
        return project;
    }
}
