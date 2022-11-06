using Sparrow.Console.Rules;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Processes.Settings;
using Sparrow.Video.Shortcuts.Processors;

namespace Sparrow.Console.Processors;

public class ScaleRuleProcessor : RuleProcessorBase<ScaleFileRule>
{
    public ScaleRuleProcessor(
        IUploadFilesService uploadFilesService,
        IPathsProvider pathsProvider,
        IScaleProcess scaleProcess)
    : base(uploadFilesService)
    {
        _pathsProvider = pathsProvider;
        _scaleProcess = scaleProcess;
    }

    private readonly IPathsProvider _pathsProvider;
    private readonly IScaleProcess _scaleProcess;

    public override ReferenceType ResultFileReferenceType => ReferenceType.InProcess;

    public override async Task<IFile> ProcessAsync(IProjectFile file, ScaleFileRule rule)
    {
        var toScaleFile = GetActualFile(file);
        var saveDirPath = _pathsProvider.GetPathFromCurrent("ScaledFiles");
        var saveSettings = new SaveSettings() {
            SaveFullPath = Path.Combine(saveDirPath, file.File.Name + file.File.Extension)
        };
        return await _scaleProcess.ScaleVideoAsync(toScaleFile, rule.Scale, saveSettings);
    }
}
