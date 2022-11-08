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
            var saveFileName = $"snapshot_{DateTime.Now.Ticks}.png";
            var fromFilePath = file.File.Path;
            var snapshotParameter = new SnapshotCommandParameters(saveFileName, fromFilePath)
            {
                Time = TimeSpan.FromSeconds(file.Analyse.StreamAnalyses.Video().Duration / (i + 1))
            };
            var source = new SnapshotCommandSource(snapshotParameter);
            lastSnapshotImage = await _ffmpegProcess.StartAsync(source, cancellationToken);
        }
        return lastSnapshotImage;
    }
}
