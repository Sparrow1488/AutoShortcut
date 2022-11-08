using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Builders;
using Sparrow.Video.Shortcuts.Builders.Formats;
using Sparrow.Video.Shortcuts.Processes.Abstractions;

namespace Sparrow.Video.Shortcuts.Processes;

public class ConcatinateProcess : FFmpegProcess, IConcatinateProcess
{
    private readonly IScriptFormatsProvider _scriptFormatsProvider;
    private IEnumerable<string> _concatinateFilesPaths;
    private ISaveSettings _saveSettings;

    public ConcatinateProcess(
        IServiceProvider services,
        IScriptFormatsProvider scriptFormatsProvider)
    : base(services)
    {
        _scriptFormatsProvider = scriptFormatsProvider;
    }

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
        IEnumerable<string> filesPaths, ISaveSettings saveSettings, CancellationToken cancellationToken = default)
    {
        _saveSettings = saveSettings;
        _concatinateFilesPaths = filesPaths;
        return await StartFFmpegAsync(cancellationToken);
    }
}
