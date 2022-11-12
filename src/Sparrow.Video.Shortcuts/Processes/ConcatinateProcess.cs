using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Builders;
using Sparrow.Video.Shortcuts.Builders.Formats;

namespace Sparrow.Video.Shortcuts.Processes
{
    public class ConcatinateProcess : FFmpegProcess, IConcatinateProcess
    {
        public ConcatinateProcess(
            ISaveService saveService,
            IPathsProvider pathsProvider,
            IConfiguration configuration,
            ILogger<FFmpegProcess> logger,
            IScriptFormatsProvider scriptFormatsProvider,
            IUploadFilesService uploadFilesService,
            IEnvironmentSettingsProvider environmentSettingsProvider)
        : base(saveService, pathsProvider, configuration, logger, uploadFilesService, environmentSettingsProvider)
        {
            _scriptFormatsProvider = scriptFormatsProvider;
        }

        private ISaveSettings _saveSettings;
        private readonly IScriptFormatsProvider _scriptFormatsProvider;
        private IEnumerable<string> _concatinateFilesPaths;

        protected override string OnConfigureFFmpegCommand()
        {
            var builder = new ScriptBuilder();
            builder.Insert("-y -f concat -safe 0");
            _concatinateFilesPaths.ToList().ForEach(x => builder.Insert($"-i \"{x}\""));
            const string presetSpeed = "slower";
            builder.InsertLast($"-c:a copy -c:v copy -preset {presetSpeed} -qp 0 \"{_saveSettings.SaveFullPath}\"");
            var concatSourcesFormat = _scriptFormatsProvider.CreateFormat<FileConcatSourcesFormat>();
            return builder.BuildScript(concatSourcesFormat).GetCommand();
        }

        protected override ISaveSettings OnConfigureSaveSettings() => _saveSettings;

        public async Task<IFile> ConcatinateFilesAsync(
            IEnumerable<string> filesPaths, ISaveSettings saveSettings)
        {
            _saveSettings = saveSettings;
            _concatinateFilesPaths = filesPaths;
            return await StartFFmpegAsync();
        }
    }
}
