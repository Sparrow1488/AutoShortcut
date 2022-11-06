using Sparrow.Console.Rules;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Processes.Settings;
using Sparrow.Video.Shortcuts.Processors;

namespace Sparrow.Console.Processors;

public class SilentAudioRuleProcessor : RuleProcessorBase<SilentFileRule>
{
    public SilentAudioRuleProcessor(
        IPathsProvider pathProvider,
        IUploadFilesService uploadFilesService,
        IMakeSilentProcess makeSilentProcess)
    : base(uploadFilesService)
    {
        _pathProvider = pathProvider;
        _uploadFilesService = uploadFilesService;
        _makeSilentProcess = makeSilentProcess;
    }

    private readonly IPathsProvider _pathProvider;
    private readonly IUploadFilesService _uploadFilesService;
    private readonly IMakeSilentProcess _makeSilentProcess;

    public override ReferenceType ResultFileReferenceType => ReferenceType.InProcess;

    public override async Task<IFile> ProcessAsync(IProjectFile file, SilentFileRule rule)
    {
        var processFile = GetActualFile(file);
        return await _makeSilentProcess.MakeSilentAsync(
                            processFile, GetSaveSettings(file.File.Name + file.File.Extension));
    }

    private ISaveSettings GetSaveSettings(string fileNameWithExtenion)
    {
        var silentPath = _pathProvider.GetPathFromCurrent("SilentFiles");
        var settings = new SaveSettings()
        {
            SaveFullPath = Path.Combine(silentPath, fileNameWithExtenion)
        };
        return settings;
    }
}
