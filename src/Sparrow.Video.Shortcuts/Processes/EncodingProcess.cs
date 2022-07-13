using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Primitives;

namespace Sparrow.Video.Shortcuts.Processes
{
    public class EncodingProcess : FFmpegProcess, IEncodingProcess
    {
        private StringPath _filePath;
        private IEncodingSettings _settings;
        private ISaveSettings _saveSettings;

        public EncodingProcess(
            ISaveService saveService, 
            IPathsProvider pathsProvider, 
            IConfiguration configuration, 
            ILogger<FFmpegProcess> logger, 
            IUploadFilesService uploadFilesService, 
            IEnvironmentSettingsProvider environmentSettingsProvider) 
        : base(saveService, pathsProvider, configuration, logger, uploadFilesService, environmentSettingsProvider)
        {
        }

        protected override string OnConfigureFFmpegCommand() =>
            $"-y -i \"{_filePath.Value}\" -acodec copy -vcodec copy -vbsf h264_mp4toannexb -crf 17 -f {_settings.EncodingType} \"{_saveSettings.SaveFullPath}\" ";
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
