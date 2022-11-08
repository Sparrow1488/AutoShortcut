using Sparrow.Console.Rules;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Processes.Settings;
using Sparrow.Video.Shortcuts.Processors;

namespace Sparrow.Console.Processors;

public class EncodingRuleProcessor : RuleProcessorBase<EncodingFileRule>
{
    private readonly IProjectSaveSettingsCreator _projectSaveSettings;
    private readonly IEncodingProcess _encodingProcess;

    public EncodingRuleProcessor(
        IUploadFilesService uploadFilesService,
        IProjectSaveSettingsCreator saveSettingsCreator,
        IEncodingProcess encodingProcess)
    : base(uploadFilesService)
    {
        _projectSaveSettings = saveSettingsCreator;
        _encodingProcess = encodingProcess;
    }

    public override ReferenceType ResultFileReferenceType => ReferenceType.InProcess;

    public override async Task<IFile> ProcessAsync(
        IProjectFile file, EncodingFileRule rule, CancellationToken cancellationToken = default)
    {
        var encodeFile = GetActualFile(file);
        var saveSettings = _projectSaveSettings.Create(
                                sectionName: ProjectConfigSections.EncodedFiles, 
                                fileName:    file.File.Name + ".ts");
        var encodingSettings = EncodingSettings.Create(rule.EncodingType);

        return await _encodingProcess.StartEncodingAsync(
            encodeFile, encodingSettings, saveSettings, cancellationToken);
    }
}
