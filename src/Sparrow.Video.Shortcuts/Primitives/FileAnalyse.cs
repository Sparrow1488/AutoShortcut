using Sparrow.Video.Abstractions.Primitives;
using System.Collections.ObjectModel;

namespace Sparrow.Video.Shortcuts.Primitives;

public class FileAnalyse : IFileAnalyse
{
    public string FileType { get; internal set; } = Abstractions.Enums.FileType.Undefined;
    public ICollection<IStreamAnalyse> StreamAnalyses { get; } = new Collection<IStreamAnalyse>();
    public IFileFormat Format { get; internal set; } = FileFormat.Default;
}