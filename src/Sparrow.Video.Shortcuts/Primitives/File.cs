using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Shortcuts.Primitives;

public class File : IFile
{
    public string Name { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public string FileType { get; set; } = Abstractions.Enums.FileType.Undefined;
    public string Path { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
}