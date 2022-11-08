using Sparrow.Console.Rules;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Processes.Sources;
using Sparrow.Video.Shortcuts.Processes.Sources.Parameters;
using Sparrow.Video.Shortcuts.Processors;

namespace Sparrow.Console.Processors;

public class ScaleRuleProcessor : RuleProcessorBase<ScaleFileRule>
{
    private readonly IFFmpegProcess _ffmpegProcess;

    public ScaleRuleProcessor(
        IUploadFilesService uploadFilesService,
        IFFmpegProcess ffmpegProcess)
    : base(uploadFilesService)
    {
        _ffmpegProcess = ffmpegProcess;
    }

    public override ReferenceType ResultFileReferenceType => ReferenceType.InProcess;

    public override async Task<IFile> ProcessAsync(
        IProjectFile file, ScaleFileRule rule, CancellationToken cancellationToken = default)
    {
        var fromFilePath = file.File.Path;
        var saveFileName = file.File.Name + file.File.Extension;
        var parameter = new ScaleCommandParameters(saveFileName, fromFilePath)
        {
            Height = rule.Resolution.Height,
            Width = rule.Resolution.Width
        };
        var source = new ScaleCommandSource(parameter);
        return await _ffmpegProcess.StartAsync(source, cancellationToken);
    }
}
