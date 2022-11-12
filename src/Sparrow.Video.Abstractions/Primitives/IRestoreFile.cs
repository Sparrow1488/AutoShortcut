namespace Sparrow.Video.Abstractions.Primitives;
public interface IRestoreFile
{
    public string RestoreFilePath { get; }
    public IProjectFile RestoredProjectFile { get; }
    public bool RestoredSuccess { get; }
}
