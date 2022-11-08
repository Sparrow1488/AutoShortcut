using Sparrow.Console.Rules;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Processes.Abstractions;
using Sparrow.Video.Shortcuts.Processes.Settings;
using Sparrow.Video.Shortcuts.Processes.Sources;
using Sparrow.Video.Shortcuts.Processes.Sources.Parameters;
using Sparrow.Video.Shortcuts.Processors;

namespace Sparrow.Console.Processors;

public class SnapshotsRuleProcessor : RuleProcessorBase<SnapshotsFileRule>
{
    private readonly IFFmpegProcess _ffmpegProcess;
    private readonly IProjectSaveSettingsCreator _projectSaveSettings;
    private readonly ITakeSnapshotProcess _takeSnapshotProcess;

    public SnapshotsRuleProcessor(
        IUploadFilesService uploadFilesService,
        IFFmpegProcess ffmpegProcess,
        IProjectSaveSettingsCreator projectSaveSettings,
        ITakeSnapshotProcess takeSnapshotProcess) 
    : base(uploadFilesService)
    {
        _ffmpegProcess = ffmpegProcess;
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
            var snapshotParameter = new SnapshotCommandParameters()
            {
                Time = TimeSpan.FromSeconds(file.Analyse.StreamAnalyses.Video().Duration / (i + 1)),
                FromFilePath = file.File.Path,
                SaveFileName = $"snapshot_{DateTime.Now.Ticks}.png"
            };
            var source = new SnapshotCommandSource(snapshotParameter);
            lastSnapshotImage = await _ffmpegProcess.StartAsync(source, cancellationToken);
        }
        return lastSnapshotImage;
    }
}
