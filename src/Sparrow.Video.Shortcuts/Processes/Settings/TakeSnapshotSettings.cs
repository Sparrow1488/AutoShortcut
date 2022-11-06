using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Processes.Settings;

public class TakeSnapshotSettings : ITakeSnapshotSettings
{
    public TimeSpan Time { get; set; }
    public IFile FromFile { get; set; }
}
