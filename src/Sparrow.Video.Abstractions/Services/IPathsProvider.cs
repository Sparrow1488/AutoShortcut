using Sparrow.Video.Abstractions.Projects;

namespace Sparrow.Video.Abstractions.Services;

public interface IPathsProvider
{
    string GetPath(string name);
    //string GetPathFromCurrent(string name);
    string GetPathFromProjectRoot(string name, IProjectRoot projectRoot);
    string GetPathFromSharedProject(string name);
}
