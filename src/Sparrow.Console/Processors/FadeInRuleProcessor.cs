using Sparrow.Console.Rules;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Processes.Sources;
using Sparrow.Video.Shortcuts.Processes.Sources.Parameters;
using Sparrow.Video.Shortcuts.Processors;

namespace Sparrow.Console.Processors;

public class FadeInRuleProcessor : RuleProcessorBase<FadeInFileRule>
{
    private readonly IFFmpegProcess _ffmpegProcess;

    public FadeInRuleProcessor(
        IUploadFilesService uploadFilesService,
        IFFmpegProcess ffmpegProcess) 
    : base(uploadFilesService)
    {
        _ffmpegProcess = ffmpegProcess;
    }

    public override ReferenceType ResultFileReferenceType => ReferenceType.InProcess;
    
    public override async Task<IFile> ProcessAsync(
        IProjectFile file, FadeInFileRule rule, CancellationToken cancellationToken = default)
    {
        var actualFile = GetActualFile(file);
        var saveFile = file.File.Name + file.File.Extension;
        var parameter = new FadeInCommandParameter(saveFile, actualFile.Path)
        {
            Seconds = rule.Seconds
        };
        var source = new FadeInCommandSource(parameter);
        var fadeInFile = await _ffmpegProcess.StartAsync(source, cancellationToken);
        return fadeInFile;
    }
}