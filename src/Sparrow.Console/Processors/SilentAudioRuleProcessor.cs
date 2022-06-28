using Sparrow.Console.Rules;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Primitives;
using Sparrow.Video.Shortcuts.Processes.Settings;
using Sparrow.Video.Shortcuts.Processors;

namespace Sparrow.Console.Processors
{
    public class SilentAudioRuleProcessor : RuleProcessorBase<SilentFileRule>
    {
        public SilentAudioRuleProcessor(
            IPathsProvider pathProvider,
            IUploadFilesService uploadFilesService,
            IMakeSilentProcess makeSilentProcess)
        {
            _pathProvider = pathProvider;
            _uploadFilesService = uploadFilesService;
            _makeSilentProcess = makeSilentProcess;
        }

        private readonly IPathsProvider _pathProvider;
        private readonly IUploadFilesService _uploadFilesService;
        private readonly IMakeSilentProcess _makeSilentProcess;

        public override async Task ProcessAsync(IProjectFile file, SilentFileRule rule)
        {
            var makeSilentFilePath = file.References.GetActual().FileFullPath;
            var processFile = _uploadFilesService.GetFile(makeSilentFilePath);
            var silentFile = await _makeSilentProcess.MakeSilentAsync(
                                processFile, GetSaveSettings(file.File.Name + file.File.Extension));
            file.References.Add(new Reference()
            {
                Name = RuleName.Silent.Value,
                FileFullPath = silentFile.Path,
                Type = ReferenceType.InProcess
            });
        }

        private ISaveSettings GetSaveSettings(string fileNameWithExtenion)
        {
            var silentPath = _pathProvider.GetPathFromCurrent("SilentFiles");
            var settings = new SaveSettings()
            {
                SaveFullPath = Path.Combine(silentPath, fileNameWithExtenion)
            };
            return settings;
        }
    }
}
