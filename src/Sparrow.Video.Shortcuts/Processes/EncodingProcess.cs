using Microsoft.Extensions.Configuration;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Primitives;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Processes
{
    public class EncodingProcess : ExecutionProcessBase, IEncodingProcess
    {
        public EncodingProcess(
            IUploadFilesService uploadFilesService,
            IConfiguration configuration)
        {
            _uploadFilesService = uploadFilesService;
            Configuration = configuration;
        }

        private StringPath _filePath;
        private IEncodingSettings? _settings;
        private readonly IUploadFilesService _uploadFilesService;

        public IConfiguration Configuration { get; }

        protected override StringPath OnGetProcessPath()
        {
            var ffmpeg = Configuration.GetRequiredSection("Processes:Video:ffmpeg").Get<string>();
            return StringPath.CreateExists(ffmpeg);
        }

        protected override ProcessSettings OnConfigureSettings()
        {
            var settings = base.OnConfigureSettings();
            settings.Argument = $"-y -i \"{_filePath.Value}\" -acodec copy -vcodec copy -vbsf h264_mp4toannexb -f {_settings.EncodingType} " + _settings.SaveSettings.SaveFullPath;
            if (Configuration.IsDebug())
            {
                settings.IsShowConsole = true;
            }
            return settings;
        }

        public async Task<IFile> StartEncodingAsync(IFile encodable, IEncodingSettings settings)
        {
            _settings = settings;
            _filePath = StringPath.CreateExists(encodable.Path);
            await StartAsync();
            return _uploadFilesService.GetFile(_settings.SaveSettings.SaveFullPath);
        }
    }
}
