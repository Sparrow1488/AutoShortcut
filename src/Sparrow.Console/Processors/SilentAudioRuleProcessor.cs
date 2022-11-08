using Sparrow.Console.Rules;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Processes.Abstractions;
using Sparrow.Video.Shortcuts.Processes.Sources;
using Sparrow.Video.Shortcuts.Processes.Sources.Parameters;
using Sparrow.Video.Shortcuts.Processors;

namespace Sparrow.Console.Processors;

public class SilentAudioRuleProcessor : RuleProcessorBase<SilentFileRule>
{
    private readonly IFFmpegProcess _ffmpegProcess;

    public SilentAudioRuleProcessor(
        IUploadFilesService uploadFilesService,
        IFFmpegProcess ffmpegProcess)
    : base(uploadFilesService)
    {
        _ffmpegProcess = ffmpegProcess;
    }

    public override ReferenceType ResultFileReferenceType => ReferenceType.InProcess;

    public override async Task<IFile> ProcessAsync(
        IProjectFile file, SilentFileRule rule, CancellationToken cancellationToken = default)
    {
        var processFile = GetActualFile(file);

        var saveFileName = file.File.Name + file.File.Extension;
        var fromFilePath = processFile.Path;
        var parameters = new SilentCommandParameters(saveFileName, fromFilePath);
        var source = new SilentCommandSource(parameters);
        return await _ffmpegProcess.StartAsync(source, cancellationToken);
    }
}
