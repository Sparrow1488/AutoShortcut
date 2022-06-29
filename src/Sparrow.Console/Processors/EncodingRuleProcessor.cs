using Sparrow.Console.Rules;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Primitives;
using Sparrow.Video.Shortcuts.Processes.Settings;
using Sparrow.Video.Shortcuts.Processors;

namespace Sparrow.Console.Processors
{
    public class EncodingRuleProcessor : RuleProcessorBase<EncodingFileRule>
    {
        public EncodingRuleProcessor(
            IUploadFilesService uploadFilesService,
            IPathsProvider pathsProvider,
            IEncodingProcess encodingProcess)
        {
            _uploadFilesService = uploadFilesService;
            _pathsProvider = pathsProvider;
            _encodingProcess = encodingProcess;
        }

        private readonly IUploadFilesService _uploadFilesService;
        private readonly IPathsProvider _pathsProvider;
        private readonly IEncodingProcess _encodingProcess;

        public override async Task ProcessAsync(IProjectFile file, EncodingFileRule rule)
        {
            var encodeActualFilePath = file.References.GetActual().FileFullPath;
            var encodeFile = _uploadFilesService.GetFile(encodeActualFilePath); // TODO: вынести в абстрактный метод

            var processedFileDirPath = _pathsProvider.GetPathFromCurrent("EncodedFiles");
            var encodedFilePath = Path.Combine(processedFileDirPath, file.File.Name + file.File.Extension); // TODO: но я сохраняю для .ts
            var saveSettings = new SaveSettings() { SaveFullPath = encodedFilePath };
            var encodingSettings = new EncodingSettings() { EncodingType = rule.EncodingType };
            var encodedFile = await _encodingProcess.StartEncodingAsync(
                                encodeFile, encodingSettings, saveSettings);
            file.References.Add(new Reference()
            {
                Name = rule.RuleName.Value,
                FileFullPath = encodedFile.Path,
                Type = ReferenceType.InProcess
            });
        }
    }
}
