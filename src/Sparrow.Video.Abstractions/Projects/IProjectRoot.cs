using Sparrow.Video.Abstractions.Projects.Options;

namespace Sparrow.Video.Abstractions.Projects;

public interface IProjectRoot
{
    IProjectPaths ProjectPaths { get; }
    IProjectRoot WithPaths(IProjectPaths paths);
}
