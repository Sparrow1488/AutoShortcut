using Microsoft.Extensions.Logging;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Services;

namespace Sparrow.Video.Shortcuts.Services
{
    public class RestoreProjectService : IRestoreProjectService
    {
        public RestoreProjectService(
            ILogger<RestoreProjectService> logger,
            IUploadFilesService uploadFilesService)
        {
            _logger = logger;
            _uploadFilesService = uploadFilesService;
        }

        private readonly ILogger<RestoreProjectService> _logger;
        private readonly IUploadFilesService _uploadFilesService;
        private string _restoreDirectoryPath = string.Empty;

        public RestoreProjectService RestoreFrom(string directoryPath)
        {
            _restoreDirectoryPath = directoryPath;
            return this;
        }

        public Task<IProject> RestoreAsync(string restoreFilesDirectoryPath)
        {
            // 1. Восстановить файлы
            // 2. Получить инфу о проекте (Rules, OutputInfo)
            // 3. Соотнести востановленные файлы с Rules
            // 4. Сравнить, не были ли изменены итоговые характеристики

            var files = _uploadFilesService.GetFiles(_restoreDirectoryPath);
            throw new NotImplementedException();
        }
    }
}
