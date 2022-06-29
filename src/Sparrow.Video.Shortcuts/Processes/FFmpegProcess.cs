using Microsoft.Extensions.Configuration;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Primitives;
using Sparrow.Video.Shortcuts.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Processes
{
    public abstract class FFmpegProcess : ExecutionProcessBase
    {
        public FFmpegProcess(
            IUploadFilesService uploadFilesService,
            IConfiguration configuration) : base(configuration)
        {
            _uploadFilesService = uploadFilesService;
        }

        private readonly IUploadFilesService _uploadFilesService;
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
