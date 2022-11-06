using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes.Settings;

namespace Sparrow.Video.Abstractions.Processes;

public interface ITakeSnapshotProcess
{
    Task<ISnapshot> TakeSnapshotAsync(
        ITakeSnapshotSettings snapshotSettings, ISaveSettings saveSettings, CancellationToken token = default);
}
