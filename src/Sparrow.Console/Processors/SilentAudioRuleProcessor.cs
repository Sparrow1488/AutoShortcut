using Sparrow.Console.Rules;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Processors;

namespace Sparrow.Console.Processors;

public class SilentAudioRuleProcessor : RuleProcessorBase<SilentFileRule>
{
    private readonly IProjectSaveSettingsCreator _projectSaveSettings;
    private readonly IMakeSilentProcess _makeSilentProcess;

    public SilentAudioRuleProcessor(
        IProjectSaveSettingsCreator projectSaveSettings,
        IUploadFilesService uploadFilesService,
        IMakeSilentProcess makeSilentProcess)
    : base(uploadFilesService)
    {
        _projectSaveSettings = projectSaveSettings;
        _makeSilentProcess = makeSilentProcess;
    }

    public override ReferenceType ResultFileReferenceType => ReferenceType.InProcess;

    public override async Task<IFile> ProcessAsync(
        IProjectFile file, SilentFileRule rule, CancellationToken cancellationToken = default)
    {
        var processFile = GetActualFile(file);
        var saveSettings = _projectSaveSettings.Create(
                                sectionName: ProjectConfigSections.SilentFiles,
                                fileName: file.File.Name + file.File.Extension);
        return await _makeSilentProcess.MakeSilentAsync(processFile, saveSettings, cancellationToken);
    }
}
