namespace Sparrow.Video.Abstractions.Primitives
{
    public interface IFile
    {
        string Name { get; }
        string Extension { get; }
        string FileType { get; }
        string Path { get; }
    }
}
