using Microsoft.Extensions.Logging;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Services;

namespace Sparrow.Video.Shortcuts.Services;

public class RestoreProjectService : IRestoreProjectService
{
    private readonly ILogger<RestoreProjectService> _logger;
    private readonly IRestoreProjectOptionsService _restoreOptionsService;
    private readonly IRestoreFilesService _restoreFilesService;
    private readonly IProjectCreator _creator;

    public RestoreProjectService(
        IProjectCreator creator,
        ILogger<RestoreProjectService> logger,
        IRestoreProjectOptionsService restoreOptionsService,
        IRestoreFilesService restoreFilesService)
    {
        _logger = logger;
        _restoreOptionsService = restoreOptionsService;
        _restoreFilesService = restoreFilesService;
        _creator = creator;
    }

    public async Task<IProject> RestoreAsync(string restoreFilesDirectoryPath, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting restore project");
        var restoredFiles = await _restoreFilesService.RestoreFilesAsync(restoreFilesDirectoryPath);
        var restoreProjectOptions = await _restoreOptionsService.RestoreOptionsAsync();

        var restoredProject = _creator.CreateProjectWithOptions(
                                    files: restoredFiles.Select(x => x.RestoredProjectFile), 
                                    options: restoreProjectOptions);

        _logger.LogInformation("Project restored");
        return restoredProject;
    }
}
