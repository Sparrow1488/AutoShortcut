using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Builders;
using Sparrow.Video.Shortcuts.Exceptions;

namespace Sparrow.Video.Shortcuts.Processes
{
    public class VideoFormatorProcess : FFmpegProcess, IFormatorProcess
    {
        public VideoFormatorProcess(
            IDefaultSaveService saveService,
            IPathsProvider pathsProvider,
            IConfiguration configuration,
            ILogger<FFmpegProcess> logger,
            IResourcesService resourcesService,
            IUploadFilesService uploadFilesService,
            IEnvironmentSettingsProvider environmentSettingsProvider)
        : base(saveService, pathsProvider, configuration, logger, uploadFilesService, environmentSettingsProvider)
        {
            _resourcesService = resourcesService;
        }

        private IFile _toProcessFile;
        private IFileAnalyse _fileAnalyse;
        private ISaveSettings _saveSettings;
        private IVideoFormatSettings _formatSettings;
        private readonly IResourcesService _resourcesService;

        public async Task<IFile> CreateInFormatAsync(
            IFile toFormat, IFileAnalyse analyse, 
                IVideoFormatSettings settings, ISaveSettings saveSettings)
        {
            _fileAnalyse = analyse;
            _toProcessFile = toFormat;
            _formatSettings = settings;
            _saveSettings = saveSettings;
            return await StartFFmpegAsync();
        }

        protected override ISaveSettings OnConfigureSaveSettings() => _saveSettings;
        protected override string OnConfigureFFmpegCommand()
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
                                 .InsertLast($"-crf 12")
                                 .InsertLast($"-r {_formatSettings.FrameFrequency.Value}")
                                 .InsertLast($"\"{_saveSettings.SaveFullPath}\"")
                                 .BuildCommand();
            return command;
        }

        private void ThrowIfNoVideoStream(IVideoStreamAnalyse streamAnalyse)
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
