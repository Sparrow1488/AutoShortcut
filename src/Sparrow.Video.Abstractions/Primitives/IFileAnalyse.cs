namespace Sparrow.Video.Abstractions.Primitives;

public interface IFileAnalyse
{
    string FileType { get; }
    ICollection<IStreamAnalyse> StreamAnalyses { get; }
    IFileFormat Format { get; }
}