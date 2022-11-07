using Microsoft.Extensions.Logging;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Primitives;
using Sparrow.Video.Shortcuts.Services.Options;

namespace Sparrow.Video.Shortcuts.Services;

public class RestoreFilesService : IRestoreFilesService
{
    private readonly ILogger<RestoreFilesService> _logger;
    private readonly IReadFileTextService _fileTextService;
    private readonly IJsonSerializer _serializer;
    private readonly IUploadFilesService _uploadFilesService;
    private readonly IProjectFileCreator _projectFileCreator;

    public RestoreFilesService(
        ILogger<RestoreFilesService> logger,
        IReadFileTextService fileTextService,
        IJsonSerializer serializer,
        IUploadFilesService uploadFilesService,
        IProjectFileCreator projectFileCreator)
    {
        _logger = logger;
        _fileTextService = fileTextService;
        _serializer = serializer;
        _uploadFilesService = uploadFilesService;
        _projectFileCreator = projectFileCreator;
    }

    public async Task<ICollection<IRestoreFile>> RestoreFilesAsync(string restoreDirectoryPath)
    {
        var restoredFilesList = new List<IRestoreFile>();
        _logger.LogInformation("Starting restore files from \"{path}\"", restoreDirectoryPath);
        var options = new UploadFilesOptions() {
            OnUploadedIgnoreFile = file => UploadFileAction.NoAction
        };
        var directoryFiles = await _uploadFilesService.GetFilesAsync(restoreDirectoryPath, options);
        var restoreFiles = directoryFiles.Where(x => x.FileType == FileType.Restore);
        _logger.LogInformation("Found restore files {count}", restoreFiles.Count());

        foreach (var restoreFile in restoreFiles)
        {
            var fileJson = await _fileTextService.ReadTextAsync(restoreFile.Path);
            var restoredProjectFile = _serializer.Deserialize<ProjectFile>(fileJson);
            RestoreFile restoredFile = new()
            {
                RestoredSuccess = true,
                RestoredProjectFile = restoredProjectFile,
                RestoreFilePath = restoreFile.Path
            };
            restoredFilesList.Add(restoredFile);
        }

        var detectedOutsideProjectFiles = SelectNewFiles(directoryFiles, restoredFilesList);
        if (detectedOutsideProjectFiles.Any())
        {
            _logger.LogInformation("Found files outside the project {count}", detectedOutsideProjectFiles.Count());
            foreach (var detectedNewFile in detectedOutsideProjectFiles)
            {
                var projectFile = await _projectFileCreator.CreateAsync(detectedNewFile);
                RestoreFile restoredFile = new()
                {
                    RestoredSuccess = false,
                    RestoredProjectFile = projectFile,
                    RestoreFilePath = detectedNewFile.Path
                };
                restoredFilesList.Add(restoredFile);
            }
        }

        return restoredFilesList;
    }

    public async Task<ICollection<IRestoreFile>> RestoreFilesAsync(IEnumerable<string> filesPaths)
    {
        var restoredFilesList = new List<IRestoreFile>();
        var files = new List<IFile>();

        //_logger.LogInformation("Starting restore files from \"{path}\"", restoreDirectoryPath);
        var options = new UploadFilesOptions()
        {
            OnUploadedIgnoreFile = file => UploadFileAction.NoAction
        };

        foreach (var filePath in filesPaths)
        {
            files.Add(_uploadFilesService.GetFile(filePath));
        }

        foreach (var restoreFile in files)
        {
            // TODO: ссылка на .restore путь
            var restorePath = restoreFile.Path.ChangeFileExtension(".restore");
            var fileJson = await _fileTextService.ReadTextAsync(restorePath);
            var restoredProjectFile = _serializer.Deserialize<ProjectFile>(fileJson);
            RestoreFile restoredFile = new()
            {
                RestoredSuccess = true,
                RestoredProjectFile = restoredProjectFile,
                RestoreFilePath = restoreFile.Path
            };
            restoredFilesList.Add(restoredFile);
        }

        var detectedOutsideProjectFiles = SelectNewFiles(files, restoredFilesList);
        if (detectedOutsideProjectFiles.Any())
        {
            _logger.LogInformation("Found files outside the project {count}", detectedOutsideProjectFiles.Count());
            foreach (var detectedNewFile in detectedOutsideProjectFiles)
            {
                var projectFile = await _projectFileCreator.CreateAsync(detectedNewFile);
                RestoreFile restoredFile = new()
                {
                    RestoredSuccess = false,
                    RestoredProjectFile = projectFile,
                    RestoreFilePath = detectedNewFile.Path
                };
                restoredFilesList.Add(restoredFile);
            }
        }

        return restoredFilesList;
    }

    private static IEnumerable<IFile> SelectNewFiles(
        IEnumerable<IFile> totalFiles,
        IEnumerable<IRestoreFile> projectRestoreFiles)
    {
        var restoredFilesPaths = projectRestoreFiles.Select(y => y.RestoredProjectFile.File.Path);
        var newFiles = totalFiles.Where(
                        file => !restoredFilesPaths.Contains(file.Path) &&
                        file.FileType == FileType.Video);
        return newFiles;
    }
}
