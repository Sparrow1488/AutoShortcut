namespace Sparrow.Video.Abstractions.Primitives
{
    public interface IFileAnalyse
    {
        public string FileType { get; }
        ICollection<IStreamAnalyse> StreamAnalyses { get; }
        IFileFormat Format { get; }
    }
}
