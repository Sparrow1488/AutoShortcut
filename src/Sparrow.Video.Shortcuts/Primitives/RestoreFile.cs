using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Shortcuts.Primitives;

public class RestoreFile : IRestoreFile
{
    public string RestoreFilePath { get; set; }
    public IProjectFile RestoredProjectFile { get; set; }
    public bool RestoredSuccess { get; set; }
}