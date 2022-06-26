namespace Sparrow.Video.Abstractions.Services
{
    public interface IPathsProvider
    {
        string GetPath(string name);
        string GetPathFromCurrent(string name);
    }
}
