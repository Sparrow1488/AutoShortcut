using Microsoft.Extensions.Logging;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Primitives;
using Sparrow.Video.Shortcuts.Services.Options;

namespace Sparrow.Video.Shortcuts.Services
{
    public class RestoreFilesService : IRestoreFilesService
    {
        public RestoreFilesService(
            ILogger<RestoreFilesService> logger,
            IReadFileTextService fileTextService,
            IJsonSerializer serializer,
            IUploadFilesService uploadFilesService)
        {
            _logger = logger;
            _fileTextService = fileTextService;
            _serializer = serializer;
            _uploadFilesService = uploadFilesService;
        }

        private readonly ILogger<RestoreFilesService> _logger;
        private readonly IReadFileTextService _fileTextService;
        private readonly IJsonSerializer _serializer;
        private readonly IUploadFilesService _uploadFilesService;

        public async Task<ICollection<IRestoreFile>> RestoreFilesAsync(string restoreDirectoryPath)
        {
            var restoredFilesList = new List<IRestoreFile>();
            _logger.LogInformation($"Starting restore files from \"{restoreDirectoryPath}\"");
            var options = new UploadFilesOptions() {
                OnUploadedIgnoreFile = file => UploadFileAction.NoAction
            };
            var directoryFiles = _uploadFilesService.GetFiles(restoreDirectoryPath, options);
            var restoreFiles = directoryFiles.Where(x => x.FileType == FileType.Restore);
            _logger.LogInformation("Found restore files " + restoreFiles.Count());

            foreach (var restoreFile in restoreFiles)
            {
                var fileJson = await _fileTextService.ReadTextAsync(restoreFile.Path);
                var restoreFileObject = _serializer.Deserialize<ProjectFile>(fileJson);
                CheckFullnessFileReferences(restoreFileObject);
                RestoreFile restoredFile = new()
                {
                    IsSuccess = true,
                    RestoredProjectFile = restoreFileObject,
                    RestoreFilePath = restoreFile.Path
                };
                restoredFilesList.Add(restoredFile);
            }
            return restoredFilesList;
        }

        private void CheckFullnessFileReferences(IProjectFile restoreProjectFile)
        {
            _logger.LogInformation("Check file reference fullness");
            foreach (var reference in restoreProjectFile.References)
            {
                if (!System.IO.File.Exists(reference.FileFullPath))
                {
                    _logger.LogWarning("File with target '{target}' is specified, but not exists",
                        reference.Target);
                }
            }
        }
    }
}
