using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Primitives;
using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Processes;

public abstract class FFmpegProcess : ExecutionProcessBase
{
    public FFmpegProcess(
        IDefaultSaveService saveService,
        IPathsProvider pathsProvider,
        IConfiguration configuration,
        IUploadFilesService uploadFilesService,
        IEnvironmentSettingsProvider environmentSettingsProvider,
        ILogger<FFmpegProcess> logger)
    : base(configuration, logger)
    {
        _saveService = saveService;
        _pathsProvider = pathsProvider;
        _uploadFilesService = uploadFilesService;
        _environmentSettingsProvider = environmentSettingsProvider;
    }

    private readonly IDefaultSaveService _saveService;
    private readonly IPathsProvider _pathsProvider;
    private readonly IUploadFilesService _uploadFilesService;
    private readonly IEnvironmentSettingsProvider _environmentSettingsProvider;
    private ISaveSettings _saveSettings;

    protected async Task<IFile> StartFFmpegAsync(CancellationToken cancellationToken = default)
    {
        await StartAsync(cancellationToken);
        return _uploadFilesService.GetFile(_saveSettings.SaveFullPath);
    }

    public override async Task StartAsync(CancellationToken cancellationToken = default)
    {
        _saveSettings = OnConfigureSaveSettings();
        var outputFileDirectoryPath = Path.GetDirectoryName(_saveSettings.SaveFullPath);
        Directory.CreateDirectory(outputFileDirectoryPath);
        await base.StartAsync(cancellationToken);
    }

    protected override async Task OnStartingProcessAsync(ProcessSettings processSettings, CancellationToken cancellationToken = default)
    {
        if (_environmentSettingsProvider.IsFFmpegScriptsLoggingEnabled())
        {
            Logger.LogDebug("FFmpegScriptsLogging is {status}. Saving executable script", EnvironmentSettings.FFmpegLogging.Enable);
            var saveScriptsPath = _pathsProvider.GetPathFromSharedProject("Scripts");
            var logFileName = GetType().Name + "_FFmpeg_" + DateTime.Now.ToString("hh.mm.ss-yyyy.MM.dd") + ".txt";
            var saveScriptSettings = new SaveSettings() { SaveFullPath = Path.Combine(saveScriptsPath, logFileName) };
            Logger.LogDebug($"Saving to \"{saveScriptSettings.SaveFullPath}\""); // TODO: сделать папку в папке со скриптами типа gen1, gen2 - чтобы понимать в каком поколении Restore проекта эти скрипты выполнялись
            await _saveService.SaveAsync(processSettings.Argument, saveScriptSettings, cancellationToken);
            Logger.LogDebug("Saved success");
        }
    }

    protected override ProcessSettings OnConfigureSettings()
    {
        var settings = base.OnConfigureSettings();
        settings.Argument = OnConfigureFFmpegCommand();
        return settings;
    }

    protected abstract string OnConfigureFFmpegCommand();
    protected abstract ISaveSettings OnConfigureSaveSettings();
    protected override StringPath OnGetProcessPath()
    {
        var ffmpegPath = Configuration.GetRequiredSection("Processes:Video:ffmpeg")
                                      .Get<string>();
        return StringPath.CreateExists(ffmpegPath);
    }
}
