using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Primitives;
using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Processes
{
    public abstract class FFmpegProcess : ExecutionProcessBase
    {
        public FFmpegProcess(
            ISaveService saveService,
            IPathsProvider pathsProvider,
            IConfiguration configuration,
            ILogger<FFmpegProcess> logger,
            IUploadFilesService uploadFilesService,
            IEnvironmentSettingsProvider environmentSettingsProvider) 
        : base(configuration)
        {
            _saveService = saveService;
            _pathsProvider = pathsProvider;
            _logger = logger;
            _uploadFilesService = uploadFilesService;
            _environmentSettingsProvider = environmentSettingsProvider;
        }

        private readonly ISaveService _saveService;
        private readonly IPathsProvider _pathsProvider;
        private readonly ILogger<FFmpegProcess> _logger;
        private readonly IUploadFilesService _uploadFilesService;
        private readonly IEnvironmentSettingsProvider _environmentSettingsProvider;
        private ISaveSettings _saveSettings;

        protected async Task<IFile> StartFFmpegAsync()
        {
            await StartAsync();
            return _uploadFilesService.GetFile(_saveSettings.SaveFullPath);
        }

        public override async Task StartAsync()
        {
            _saveSettings = OnConfigureSaveSettings();
            var outputFileDirectoryPath = Path.GetDirectoryName(_saveSettings.SaveFullPath);
            Directory.CreateDirectory(outputFileDirectoryPath);
            await base.StartAsync();
        }

        protected override async Task OnStartingProcessAsync(ProcessSettings processSettings, CancellationToken cancellationToken = default)
        {
            if (_environmentSettingsProvider.IsFFmpegScriptsLoggingEnabled())
            {
                _logger.LogDebug("FFmpegScriptsLogging is {status}. Saving executable script", EnvironmentSettings.FFmpegLogging.Enable);
                var saveScriptsPath = _pathsProvider.GetPathFromCurrent("Scripts");
                var logFileName = GetType().Name + "_FFmpeg_" + DateTime.Now.ToString("hh.mm.ss-yyyy.MM.dd") + ".txt";
                var saveScriptSettings = new SaveSettings() { SaveFullPath = Path.Combine(saveScriptsPath, logFileName) };
                _logger.LogDebug($"Saving to \"{saveScriptSettings.SaveFullPath}\""); // TODO: сделать папку в папке со скриптами типа gen1, gen2 - чтобы понимать в каком поколении Restore проекта эти скрипты выполнялись
                await _saveService.SaveTextAsync(processSettings.Argument, saveScriptSettings, cancellationToken);
                _logger.LogDebug("Saved success");
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
}
