namespace Sparrow.Video.Abstractions.Primitives
{
    public interface IFilesStructure
    {
        IEnumerable<IProjectFile> GetStructuredFiles(IEnumerable<IProjectFile> files);
    }
}
