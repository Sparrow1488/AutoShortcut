using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Processes;

public class MakeSilentProcess : FFmpegProcess, IMakeSilentProcess
{
    private IFile _toProcessFile;
    private ISaveSettings _saveSettings;

    public MakeSilentProcess(IServiceProvider services)
    : base(services)
    {
    }

    protected override string OnConfigureFFmpegCommand() =>
        $"-f lavfi -i anullsrc=channel_layout=stereo:sample_rate=44100 -i \"{_toProcessFile.Path}\" -c:v copy -c:a aac -shortest \"{_saveSettings.SaveFullPath}\"";
    protected override ISaveSettings OnConfigureSaveSettings() => _saveSettings;

    public async Task<IFile> MakeSilentAsync(IFile file, ISaveSettings saveSettings, CancellationToken cancellationToken = default)
    {
        _saveSettings = saveSettings;
        _toProcessFile = file;
        return await StartFFmpegAsync(cancellationToken);
    }
}
