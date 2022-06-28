using Microsoft.Extensions.Configuration;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Primitives;
using Sparrow.Video.Shortcuts.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Processes
{
    public class EncodingProcess : ExecutionProcessBase, IEncodingProcess
    {
        public EncodingProcess(
            IUploadFilesService uploadFilesService,
            IConfiguration configuration) : base(configuration)
        {
            _uploadFilesService = uploadFilesService;
        }

        private StringPath _filePath;
        private IEncodingSettings? _settings;
        private ISaveSettings _saveSettings;
        private readonly IUploadFilesService _uploadFilesService;

        protected override StringPath OnGetProcessPath()
        {
            var ffmpeg = Configuration.GetRequiredSection("Processes:Video:ffmpeg").Get<string>();
            return StringPath.CreateExists(ffmpeg);
        }

        protected override ProcessSettings OnConfigureSettings()
        {
            var settings = base.OnConfigureSettings();
            settings.Argument = $"-y -i \"{_filePath.Value}\" -acodec copy -vcodec copy -vbsf h264_mp4toannexb -f {_settings.EncodingType} " + _saveSettings.SaveFullPath;
            return settings;
        }

        public async Task<IFile> StartEncodingAsync(
            IFile encodable, IEncodingSettings settings, ISaveSettings saveSettings)
        {
            _settings = settings;
            _saveSettings = saveSettings;
            _filePath = StringPath.CreateExists(encodable.Path);
            await StartAsync();
            return _uploadFilesService.GetFile(saveSettings.SaveFullPath);
        }
    }
}
