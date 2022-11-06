using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Abstractions.Processes.Settings;

public interface ITakeSnapshotSettings
{
    TimeSpan Time { get; }
    IFile FromFile { get; }
}
