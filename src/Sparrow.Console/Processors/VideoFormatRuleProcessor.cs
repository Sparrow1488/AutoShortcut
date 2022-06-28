using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processors;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Primitives;
using Sparrow.Video.Shortcuts.Processors;
using Sparrow.Video.Shortcuts.Rules;

namespace Sparrow.Console.Processors
{
    public class VideoFormatRuleProcessor : RuleProcessorBase<VideoFormatFileRule>
    {
        public VideoFormatRuleProcessor(
            IUploadFilesService uploadFilesService,
            IFormatorProcess formatorProcess)
        {
            _uploadFilesService = uploadFilesService;
            _formatorProcess = formatorProcess;
        }

        private readonly IUploadFilesService _uploadFilesService;
        private readonly IFormatorProcess _formatorProcess;

        public override async Task ProcessAsync(IProjectFile file, VideoFormatFileRule rule)
        {
            var processFileReference = file.References.GetActual();
            var processFile = _uploadFilesService.GetFile(processFileReference.FileFullPath);
            var formattedFile = await _formatorProcess.CreateInFormatAsync(
                                    processFile, file.Analyse, rule.FormatSettings);
            file.References.Add(new Reference()
            {
                Name = RuleName.Formating.Value,
                FileFullPath = formattedFile.Path,
                Type = ReferenceType.InProcess
            });
        }
    }
}
