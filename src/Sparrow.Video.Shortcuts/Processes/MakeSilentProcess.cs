using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Services;

namespace Sparrow.Video.Shortcuts.Processes;

public class MakeSilentProcess : FFmpegProcess, IMakeSilentProcess
{
    public MakeSilentProcess(
        IDefaultSaveService saveService,
        IPathsProvider pathsProvider,
        IConfiguration configuration,
        ILogger<FFmpegProcess> logger,
        IUploadFilesService uploadFilesService,
        IEnvironmentSettingsProvider environmentSettingsProvider)
    : base(saveService, pathsProvider, configuration, uploadFilesService, environmentSettingsProvider, logger)
    {
    }

    private IFile _toProcessFile;
    private ISaveSettings _saveSettings;

    protected override string OnConfigureFFmpegCommand() =>
        $"-y -f lavfi -i anullsrc=channel_layout=stereo:sample_rate=44100 -i \"{_toProcessFile.Path}\" -c:v copy -c:a aac -shortest \"{_saveSettings.SaveFullPath}\"";
    protected override ISaveSettings OnConfigureSaveSettings() => _saveSettings;

    public async Task<IFile> MakeSilentAsync(IFile file, ISaveSettings saveSettings, CancellationToken cancellationToken = default)
    {
        _saveSettings = saveSettings;
        _toProcessFile = file;
        return await StartFFmpegAsync(cancellationToken);
    }
}
