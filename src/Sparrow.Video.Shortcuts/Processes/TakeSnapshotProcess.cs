using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Shortcuts.Primitives;

namespace Sparrow.Video.Shortcuts.Processes;

public class TakeSnapshotProcess : FFmpegProcess, ITakeSnapshotProcess
{
    private ITakeSnapshotSettings _snapshotSettings;
    private ISaveSettings _saveSettings;

    public TakeSnapshotProcess(IServiceProvider services)
    : base(services)
    {
    }

    public async Task<ISnapshot> TakeSnapshotAsync(
        ITakeSnapshotSettings snapshotSettings, ISaveSettings saveSettings, CancellationToken token = default)
    {
        _snapshotSettings = snapshotSettings;
        _saveSettings = saveSettings;
        var snapshot = await StartFFmpegAsync(token);
        return new Snapshot() { File = snapshot };
    }

    protected override string OnConfigureFFmpegCommand()
        => $"-ss {_snapshotSettings.Time.ToString(@"hh\:mm\:ss")} -i \"{_snapshotSettings.FromFile.Path}\" -frames:v 1 -q:v 2 \"{_saveSettings.SaveFullPath}\"";

    protected override ISaveSettings OnConfigureSaveSettings()
        => _saveSettings;
}
