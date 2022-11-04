using Microsoft.Extensions.Logging;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Projects.Options;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Projects;

namespace Sparrow.Video.Shortcuts.Services
{
    public class RestoreProjectService : IRestoreProjectService
    {
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

        private readonly ILogger<RestoreProjectService> _logger;
        private readonly IRestoreProjectOptionsService _restoreOptionsService;
        private readonly IRestoreFilesService _restoreFilesService;
        private readonly IProjectCreator _creator;

        public async Task<IProject> RestoreAsync(string restoreFilesDirectoryPath, CancellationToken cancellationToken = default)
        {
            // 1. Восстановить файлы +
            // 2. Получить инфу о проекте - ProjectOptions +
            // 3. Соотнести востановленные файлы с Rules (Проверить сколько новых файлов и сколько есть .restore)
            // 4. Сравнить, не были ли изменены итоговые характеристики

            _logger.LogInformation("Starting restore project");
            var restoredFiles = await _restoreFilesService.RestoreFilesAsync(restoreFilesDirectoryPath);
            var restoreProjectOptions = await _restoreOptionsService.RestoreOptionsAsync();

            var restoredProject = _creator.CreateProjectWithOptions(
                                    restoredFiles.Select(x => x.RestoredProjectFile), restoreProjectOptions);

            _logger.LogInformation("Project restored");
            return restoredProject;
        }
    }
}
