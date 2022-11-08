using Sparrow.Console.Rules;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Processes.Settings;
using Sparrow.Video.Shortcuts.Processors;

namespace Sparrow.Console.Processors;

public class SnapshotsRuleProcessor : RuleProcessorBase<SnapshotsFileRule>
{
    private readonly IPathsProvider _pathsProvider;
    private readonly IProjectSaveSettingsCreator _projectSaveSettings;
    private readonly ITakeSnapshotProcess _takeSnapshotProcess;

    public SnapshotsRuleProcessor(
        IUploadFilesService uploadFilesService,
        IProjectSaveSettingsCreator projectSaveSettings,
        ITakeSnapshotProcess takeSnapshotProcess) : base(uploadFilesService)
    {
        _projectSaveSettings = projectSaveSettings;
        _takeSnapshotProcess = takeSnapshotProcess;
    }

    public override ReferenceType ResultFileReferenceType => ReferenceType.Ignore;

    public override async Task<IFile> ProcessAsync(
        IProjectFile file, SnapshotsFileRule rule, CancellationToken cancellationToken = default)
    {
        IFile lastSnapshotImage = null!;
        for (int i = 0; i < rule.Count; i++)
        {
            var snapshotSettings = new TakeSnapshotSettings()
            {
                Time = TimeSpan.FromSeconds(file.Analyse.StreamAnalyses.Video().Duration / (i + 1)),
                FromFile = file.File
            };
            var saveSettings = _projectSaveSettings.Create(
                                sectionName: ProjectConfigSections.Snapshots,
                                fileName: $"snapshot_{DateTime.Now.Ticks}.png");
            var snapshot = await _takeSnapshotProcess.TakeSnapshotAsync(snapshotSettings, saveSettings, cancellationToken);
            lastSnapshotImage = snapshot.File;
        }
        return lastSnapshotImage;
    }
}
