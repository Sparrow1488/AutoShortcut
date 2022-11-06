using Sparrow.Console.Rules;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Processes.Settings;
using Sparrow.Video.Shortcuts.Processors;

namespace Sparrow.Console.Processors;

public class EncodingRuleProcessor : RuleProcessorBase<EncodingFileRule>
{
    public EncodingRuleProcessor(
        IUploadFilesService uploadFilesService,
        IPathsProvider pathsProvider,
        IEncodingProcess encodingProcess)
    : base(uploadFilesService)
    {
        _pathsProvider = pathsProvider;
        _encodingProcess = encodingProcess;
    }

    private readonly IPathsProvider _pathsProvider;
    private readonly IEncodingProcess _encodingProcess;

    public override ReferenceType ResultFileReferenceType => ReferenceType.InProcess;

    public override async Task<IFile> ProcessAsync(
        IProjectFile file, EncodingFileRule rule, CancellationToken cancellationToken = default)
    {
        var encodeFile = GetActualFile(file);
        var processedFileDirPath = _pathsProvider.GetPathFromSharedProject("EncodedFiles");
        var encodedFilePath = Path.Combine(processedFileDirPath, file.File.Name + ".ts"); // TODO: но я сохраняю для .ts
        var saveSettings = new SaveSettings() { SaveFullPath = encodedFilePath };
        var encodingSettings = new EncodingSettings() { EncodingType = rule.EncodingType };
        return await _encodingProcess.StartEncodingAsync(encodeFile, encodingSettings, saveSettings, cancellationToken);
    }
}
