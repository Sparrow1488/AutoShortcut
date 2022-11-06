using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Shortcuts.Primitives;

public class Snapshot : ISnapshot
{
    public IFile File { get; set; }
}
