using Sparrow.Video.Abstractions.Builders.Formats;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Builders.Formats
{
    public class FileConcatSourcesFormat : IScriptBuilderFormat
    {
        public FileConcatSourcesFormat(
            IPathsProvider pathsProvider,
            IDefaultSaveService saveService)
        {
            _pathsProvider = pathsProvider;
            _saveService = saveService;
        }

        private readonly IPathsProvider _pathsProvider;
        private readonly IDefaultSaveService _saveService;
        private string _savedConcatedFilesPath;

        public IEnumerable<string> UseFormat(IEnumerable<string> inputBuilderCommand)
        {
            var inputCommandsFormatted = new List<string>();
            var cleanSourcePaths = new List<string>();
            var sourcePaths = inputBuilderCommand.Where(x => x.StartsWith("-i")).ToList();
            sourcePaths.ForEach(x => cleanSourcePaths.Add(x.Replace("-i", "").Replace("\"", "").Trim()));
            SaveConcatedSourcesFileAsync(cleanSourcePaths)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
            inputCommandsFormatted.AddRange(inputBuilderCommand.Where(x => !x.StartsWith("-i")));
            inputCommandsFormatted.Add($"-i \"{_savedConcatedFilesPath}\"");
            return inputCommandsFormatted;
        }

        private async Task SaveConcatedSourcesFileAsync(IEnumerable<string> filesPaths)
        {
            var storeFilePath = _pathsProvider.GetPathFromSharedProject("Scripts");
            _savedConcatedFilesPath = Path.Combine(storeFilePath, "concated.txt");
            var saveSettings = new SaveSettings()
            {
                SaveFullPath = _savedConcatedFilesPath
            };
            var saveText = string.Join("\n", filesPaths.Select(x => $"file '{x}'"));
            await _saveService.SaveAsync(saveText, saveSettings);
        }
    }
}
