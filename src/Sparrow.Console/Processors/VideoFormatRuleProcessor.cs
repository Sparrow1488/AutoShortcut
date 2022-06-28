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
    public class VideoFormatRuleProcessor : RuleProcessorBase<FormatFileRule>
    {
        public VideoFormatRuleProcessor(
            IPathsProvider pathProvider,
            IUploadFilesService uploadFilesService,
            IFormatorProcess formatorProcess)
        {
            _pathProvider = pathProvider;
            _uploadFilesService = uploadFilesService;
            _formatorProcess = formatorProcess;
        }

        private readonly IPathsProvider _pathProvider;
        private readonly IUploadFilesService _uploadFilesService;
        private readonly IFormatorProcess _formatorProcess;

        public override async Task ProcessAsync(IProjectFile file, FormatFileRule rule)
        {
            var processFileReference = file.References.GetActual();
            var processFile = _uploadFilesService.GetFile(processFileReference.FileFullPath);
            var formattedFile = await _formatorProcess.CreateInFormatAsync(
                                    processFile, file.Analyse, rule.FormatSettings,
                                        GetSaveSettings(file.File.Name + file.File.Extension));
            file.References.Add(new Reference()
            {
                Name = RuleName.Formating.Value,
                FileFullPath = formattedFile.Path,
                Type = ReferenceType.InProcess
            });
        }

        private ISaveSettings GetSaveSettings(string fileNameWithExtension)
        {
            var formatterPath = _pathProvider.GetPathFromCurrent("FormattedFiles");
            var saveSettings = new SaveSettings()
            {
                SaveFullPath = Path.Combine(formatterPath, fileNameWithExtension),
            };
            return saveSettings;
        }
    }
}
