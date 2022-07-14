using Microsoft.Extensions.Logging;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Projects;

namespace Sparrow.Video.Shortcuts.Services
{
    public class RestoreProjectService : IRestoreProjectService
    {
        public RestoreProjectService(
            ILogger<RestoreProjectService> logger,
            IRestoreFilesService restoreFilesService,
            IUploadFilesService uploadFilesService)
        {
            _logger = logger;
            _restoreFilesService = restoreFilesService;
            _uploadFilesService = uploadFilesService;
        }

        private readonly ILogger<RestoreProjectService> _logger;
        private readonly IRestoreFilesService _restoreFilesService;
        private readonly IUploadFilesService _uploadFilesService;

        public async Task<IProject> RestoreAsync(string restoreFilesDirectoryPath, CancellationToken cancellationToken = default)
        {
            // 1. Восстановить файлы +
            // 2. Получить инфу о проекте (Rules, OutputInfo)
            // 3. Соотнести востановленные файлы с Rules
            // 4. Сравнить, не были ли изменены итоговые характеристики

            var restoredFiles = await _restoreFilesService.RestoreFilesAsync(restoreFilesDirectoryPath);
            var project = new ShortcutProject()
            {
                Files = restoredFiles.Select(X => X.RestoredProjectFile)
            }.Named("NOT-RESTORED-NAME");
            return project;
        }
    }
}
