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
    public class ScaleRuleProcessor : RuleProcessorBase<ScaleFileRule>
    {
        public ScaleRuleProcessor(
            IUploadFilesService uploadFilesService,
            IPathsProvider pathsProvider,
            IScaleProcess scaleProcess)
        {
            _uploadFilesService = uploadFilesService;
            _pathsProvider = pathsProvider;
            _scaleProcess = scaleProcess;
        }

        private readonly IUploadFilesService _uploadFilesService;
        private readonly IPathsProvider _pathsProvider;
        private readonly IScaleProcess _scaleProcess;

        public override async Task ProcessAsync(IProjectFile file, ScaleFileRule rule)
        {
            var actualFileRef = file.References.GetActual();
            var actualFile = _uploadFilesService.GetFile(actualFileRef.FileFullPath);
            var saveDirPath = _pathsProvider.GetPathFromCurrent("ScaledFiles");
            var saveSettings = new SaveSettings() {
                SaveFullPath = Path.Combine(saveDirPath, file.File.Name + file.File.Extension)
            };
            var result = await _scaleProcess.ScaleVideoAsync(actualFile, rule.Scale, saveSettings);
            file.References.Add(new Reference() { 
                Name = result.Name,
                Type = ReferenceType.InProcess,
                FileFullPath = result.Path
            });
        }
    }
}
