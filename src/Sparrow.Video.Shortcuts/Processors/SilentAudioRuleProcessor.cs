using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processors;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Primitives;
using Sparrow.Video.Shortcuts.Rules;

namespace Sparrow.Video.Shortcuts.Processors
{
    public class SilentAudioRuleProcessor : IRuleProcessor<SilentAudioRule>
    {
        public SilentAudioRuleProcessor(
            IUploadFilesService uploadFilesService,
            IMakeSilentProcess makeSilentProcess)
        {
            _uploadFilesService = uploadFilesService;
            _makeSilentProcess = makeSilentProcess;
        }

        private readonly IUploadFilesService _uploadFilesService;
        private readonly IMakeSilentProcess _makeSilentProcess;

        public async Task ProcessAsync(IProjectFile file, SilentAudioRule rule)
        {
            var makeSilentFilePath = file.References.GetActual().FileFullPath;
            var processFile = _uploadFilesService.GetFile(makeSilentFilePath);
            var silentFile = await _makeSilentProcess.MakeSilentAsync(
                                processFile, rule.SaveSettings);
            file.References.Add(new Reference()
            {
                Name = RuleName.Silent.Value,
                FileFullPath = silentFile.Path,
                Type = ReferenceType.InProcess
            });
        }
    }
}
