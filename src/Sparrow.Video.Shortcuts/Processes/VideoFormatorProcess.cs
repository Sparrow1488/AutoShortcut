using Microsoft.Extensions.Configuration;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Builders;
using Sparrow.Video.Shortcuts.Exceptions;
using Sparrow.Video.Shortcuts.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Processes
{
    public class VideoFormatorProcess : FFmpegProcess, IFormatorProcess
    {
        public VideoFormatorProcess(
            IUploadFilesService uploadFilesService,
            IResourcesService resourcesService,
            IConfiguration configuration) : base(configuration)
        {
            _uploadFilesService = uploadFilesService;
            _resourcesService = resourcesService;
        }

        private IFileAnalyse _fileAnalyse;
        private IFile _toProcessFile;
        private IVideoFormatSettings _formatSettings;
        private ISaveSettings _saveSettings;
        private readonly IUploadFilesService _uploadFilesService;
        private readonly IResourcesService _resourcesService;

        protected override ProcessSettings OnConfigureSettings()
        {
            var settings = base.OnConfigureSettings();
            settings.Argument = CreateProcessArgument();
            var directoryPath = Path.GetDirectoryName(_saveSettings.SaveFullPath);
            Directory.CreateDirectory(directoryPath);
            return settings;
        }

        public async Task<IFile> CreateInFormatAsync(
            IFile toFormat, IFileAnalyse analyse, 
                IVideoFormatSettings settings, ISaveSettings saveSettings)
        {
            _fileAnalyse = analyse;
            _toProcessFile = toFormat;
            _formatSettings = settings;
            _saveSettings = saveSettings;
            await StartAsync();
            return _uploadFilesService.GetFile(_saveSettings.SaveFullPath);
        }

        private string CreateProcessArgument()
        {
            var builder = new ScriptBuilder();
            var videoStream = _fileAnalyse.StreamAnalyses.FirstOrDefault(x => x.CodecType.ToLower() == "video")
                                as IVideoStreamAnalyse;
            ThrowIfNoVideoStream(videoStream);
            var videoScaleArgument = SelectResolutionCommandArgument(videoStream);
            var resolutionBackgroundResource = 
                _resourcesService.GetRequiredResource(
                    $"Resources:backgrounds:black:files:{_formatSettings.DisplayResolution.Value}");
            var command = builder.Insert($"-y -i \"{resolutionBackgroundResource.Path}\"")
                                 .Insert($"-i \"{_toProcessFile.Path}\"")
                                 .Insert($"-filter_complex \"[1:v]scale={videoScaleArgument}[v2];[0:v][v2]overlay=(main_w - overlay_w)/2:(main_h - overlay_h)/2\"")
                                 .InsertLast($"-r 60")
                                 .InsertLast($"\"{_saveSettings.SaveFullPath}\"")
                                 .BuildCommand();
            return command;
        }

        private void ThrowIfNoVideoStream(IVideoStreamAnalyse? streamAnalyse)
        {
            if (streamAnalyse is null)
            {
                throw new StreamNotFoundException(
                    "Failed to change video format. " +
                    "File analyse not contains any stream with video type");
            }
        }

        private string SelectResolutionCommandArgument(IVideoStreamAnalyse streamAnalyse)
        {
            if (streamAnalyse.Width > streamAnalyse.Height)
                return $"{_formatSettings.DisplayResolution.Width}:-1";
            else return $"-1:{_formatSettings.DisplayResolution.Height}";
        }
    }
}
