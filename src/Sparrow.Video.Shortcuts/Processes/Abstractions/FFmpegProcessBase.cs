using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Primitives;
using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Processes.Abstractions;

public abstract class FFmpegProcessBase : ExecutionProcessBase
{
    private readonly IDefaultSaveService _saveService;
    private readonly IProjectSaveSettingsCreator _projectSaveSettings;
    private readonly IUploadFilesService _uploadFilesService;
    private readonly IEnvironmentSettingsProvider _environmentSettingsProvider;
    private ISaveSettings _saveSettings;

    public FFmpegProcessBase(IServiceProvider services) : base(services)
    {
        _saveService = services.GetRequiredService<IDefaultSaveService>();
        _projectSaveSettings = services.GetRequiredService<IProjectSaveSettingsCreator>();
        _uploadFilesService = services.GetRequiredService<IUploadFilesService>();
        _environmentSettingsProvider = services.GetRequiredService<IEnvironmentSettingsProvider>();
    }

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
            var logFileName = GetType().Name + "_FFmpeg_" + DateTime.Now.ToString("hh.mm.ss-yyyy.MM.dd") + ".txt";
            var saveSettings = _projectSaveSettings.Create(
                                    sectionName: ProjectConfigSections.Scripts,
                                    fileName: logFileName);
            Logger.LogDebug($"Saving to \"{saveSettings.SaveFullPath}\""); // TODO: сделать папку в папке со скриптами типа gen1, gen2 - чтобы понимать в каком поколении Restore проекта эти скрипты выполнялись
            await _saveService.SaveAsync(processSettings.Argument, saveSettings, cancellationToken);
            Logger.LogDebug("Saved success");
        }
    }

    protected override ProcessSettings OnConfigureSettings()
    {
        var settings = base.OnConfigureSettings();
        string overwriteOutputFileFlag = string.Empty;
        settings.Argument = overwriteOutputFileFlag + OnConfigureFFmpegCommand();
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
