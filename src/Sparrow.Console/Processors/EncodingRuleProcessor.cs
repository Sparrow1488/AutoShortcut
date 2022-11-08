using Sparrow.Console.Rules;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Processes.Sources;
using Sparrow.Video.Shortcuts.Processes.Sources.Parameters;
using Sparrow.Video.Shortcuts.Processors;

namespace Sparrow.Console.Processors;

public class EncodingRuleProcessor : RuleProcessorBase<EncodingFileRule>
{
    private readonly IFFmpegProcess _ffmpegProcess;

    public EncodingRuleProcessor(
        IUploadFilesService uploadFilesService,
        IFFmpegProcess ffmpegProcess)
    : base(uploadFilesService)
    {
        _ffmpegProcess = ffmpegProcess;
    }

    public override ReferenceType ResultFileReferenceType => ReferenceType.InProcess;

    public override async Task<IFile> ProcessAsync(
        IProjectFile file, EncodingFileRule rule, CancellationToken cancellationToken = default)
    {
        var encodeFile = GetActualFile(file);

        var fromFilePath = encodeFile.Path;
        var saveFileName = file.File.Name + ".ts"; // TODO: Нужно бы как-то определять
        var parameters = new EncodingCommandParameters(saveFileName, fromFilePath)
        {
            EncodingType = rule.EncodingType
        };
        var source = new EncodingCommandSource(parameters);
        return await _ffmpegProcess.StartAsync(source, cancellationToken);
    }
}
