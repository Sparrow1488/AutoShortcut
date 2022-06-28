using Microsoft.Extensions.Configuration;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Builders;
using Sparrow.Video.Shortcuts.Builders.Formats;
using Sparrow.Video.Shortcuts.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Processes
{
    public class ConcatinateProcess : FFmpegProcess, IConcatinateProcess
    {
        public ConcatinateProcess(
            IScriptFormatsProvider scriptFormatsProvider,
            IUploadFilesService uploadFilesService,
            IConfiguration configuration) : base(configuration)
        {
            _scriptFormatsProvider = scriptFormatsProvider;
            _uploadFilesService = uploadFilesService;
        }

        private ISaveSettings _saveSettings;
        private readonly IScriptFormatsProvider _scriptFormatsProvider;
        private readonly IUploadFilesService _uploadFilesService;
        private IEnumerable<string> _concatinateFilesPaths;

        protected override ProcessSettings OnConfigureSettings()
        {
            var settings = base.OnConfigureSettings();
            settings.Argument = CreateConcationationCommand();
            var saveFileDirectory = Path.GetDirectoryName(_saveSettings.SaveFullPath);
            Directory.CreateDirectory(saveFileDirectory);
            return settings;
        }

        private string CreateConcationationCommand()
        {
            var builder = new ScriptBuilder();
            builder.Insert("-y -f concat -safe 0");
            _concatinateFilesPaths.ToList().ForEach(x => builder.Insert($"-i \"{x}\""));
            builder.InsertLast($"-c:a copy -c:v copy -preset fast -vsync cfr -r 45 \"{_saveSettings.SaveFullPath}\"");
            var concatSourcesFormat = _scriptFormatsProvider.CreateFormat<FileConcatSourcesFormat>();
            return builder.BuildScript(concatSourcesFormat).GetCommand();
        }

        public async Task<IFile> ConcatinateFilesAsync(
            IEnumerable<string> filesPaths, ISaveSettings saveSettings)
        {
            _saveSettings = saveSettings;
            _concatinateFilesPaths = filesPaths;
            await StartAsync();
            var resultFile = _uploadFilesService.GetFile(_saveSettings.SaveFullPath);
            return resultFile;
        }
    }
}
