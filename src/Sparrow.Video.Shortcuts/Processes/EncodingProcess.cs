using Microsoft.Extensions.Configuration;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Primitives;

namespace Sparrow.Video.Shortcuts.Processes
{
    public class EncodingProcess : FFmpegProcess, IEncodingProcess
    {
        public EncodingProcess(
            IUploadFilesService uploadFilesService, 
            IConfiguration configuration) : base(uploadFilesService, configuration)
        {
        }

        private StringPath _filePath;
        private IEncodingSettings _settings;
        private ISaveSettings _saveSettings;

        protected override string OnConfigureFFmpegCommand() =>
            $"-y -i \"{_filePath.Value}\" -acodec copy -vcodec copy -vbsf h264_mp4toannexb -f {_settings.EncodingType} \"{_saveSettings.SaveFullPath}\" ";
        protected override ISaveSettings OnConfigureSaveSettings() => _saveSettings;

        public async Task<IFile> StartEncodingAsync(
            IFile encodable, IEncodingSettings settings, ISaveSettings saveSettings)
        {
            _settings = settings;
            _saveSettings = saveSettings;
            _filePath = StringPath.CreateExists(encodable.Path);
            return await StartFFmpegAsync();
        }
    }
}
