using Sparrow.Console.Rules;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Processes.Sources;
using Sparrow.Video.Shortcuts.Processes.Sources.Parameters;
using Sparrow.Video.Shortcuts.Processors;

namespace Sparrow.Console.Processors;

public class SnapshotsRuleProcessor : RuleProcessorBase<SnapshotsFileRule>
{
    private readonly IFFmpegProcess _ffmpegProcess;

    public SnapshotsRuleProcessor(
        IUploadFilesService uploadFilesService,
        IFFmpegProcess ffmpegProcess)
    : base(uploadFilesService)
    {
        _ffmpegProcess = ffmpegProcess;
    }

    public override ReferenceType ResultFileReferenceType => ReferenceType.Ignore;

    public override async Task<IFile> ProcessAsync(
        IProjectFile file, SnapshotsFileRule rule, CancellationToken cancellationToken = default)
    {
        IFile lastSnapshotImage = null!;
        for (int i = 0; i < rule.Count; i++)
        {
            var saveFileName = $"snapshot_{file.File.Name}_{i}.png";
            var fromFilePath = file.File.Path;

            var durationPercentValue = file.Analyse.StreamAnalyses.Video().Duration / 100;
            var durationPercentStep = 100 / rule.Count;
            var snapshotParameter = new SnapshotCommandParameters(saveFileName, fromFilePath)
            {
                Time = TimeSpan.FromSeconds(durationPercentValue * (i+1) * durationPercentStep)
            };
            var source = new SnapshotCommandSource(snapshotParameter);
            lastSnapshotImage = await _ffmpegProcess.StartAsync(source, cancellationToken);
        }
        return lastSnapshotImage;
    }
}
