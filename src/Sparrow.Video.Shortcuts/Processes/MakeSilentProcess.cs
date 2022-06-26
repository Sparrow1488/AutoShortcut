using Microsoft.Extensions.Configuration;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Processes
{
    public class MakeSilentProcess : FFmpegProcess, IMakeSilentProcess
    {
        public MakeSilentProcess(
            IUploadFilesService uploadFilesService,
            IConfiguration configuration) : base(configuration)
        {
            _uploadFilesService = uploadFilesService;
        }

        private ISaveSettings? _saveSettings;
        private IFile? _toProcessFile;
        private readonly IUploadFilesService _uploadFilesService;

        protected override ProcessSettings OnConfigureSettings()
        {
            var settings = base.OnConfigureSettings();
            settings.Argument = $"-y -f lavfi -i anullsrc=channel_layout=stereo:sample_rate=44100 -i \"{_toProcessFile.Path}\" -c:v copy -c:a aac -shortest \"{_saveSettings.SaveFullPath}\"";
            return settings;
        }

        public async Task<IFile> MakeSilentAsync(IFile file, ISaveSettings saveSettings)
        {
            _saveSettings = saveSettings;
            _toProcessFile = file;
            await StartAsync();
            return _uploadFilesService.GetFile(_saveSettings.SaveFullPath);
        }
    }
}
