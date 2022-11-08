using Sparrow.Console.Rules;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Processes.Settings;
using Sparrow.Video.Shortcuts.Processors;

namespace Sparrow.Console.Processors;

public class ScaleRuleProcessor : RuleProcessorBase<ScaleFileRule>
{
    private readonly IProjectSaveSettingsCreator _projectSaveSettings;
    private readonly IScaleProcess _scaleProcess;

    public ScaleRuleProcessor(
        IUploadFilesService uploadFilesService,
        IProjectSaveSettingsCreator projectSaveSettings,
        IScaleProcess scaleProcess)
    : base(uploadFilesService)
    {
        _projectSaveSettings = projectSaveSettings;
        _scaleProcess = scaleProcess;
    }

    public override ReferenceType ResultFileReferenceType => ReferenceType.InProcess;

    public override async Task<IFile> ProcessAsync(
        IProjectFile file, ScaleFileRule rule, CancellationToken cancellationToken = default)
    {
        var toScaleFile = GetActualFile(file);
        var saveSettings = _projectSaveSettings.Create(
                                sectionName: ProjectConfigSections.ScaledFiles,
                                fileName: file.File.Name + file.File.Extension);
        return await _scaleProcess.ScaleVideoAsync(toScaleFile, rule.Scale, saveSettings, cancellationToken);
    }
}
