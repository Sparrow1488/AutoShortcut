using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects.Options;

namespace Sparrow.Video.Abstractions.Projects;

public interface IProject
{
    string Name { get; }
    IProjectOptions Options { get; }
    IEnumerable<IProjectFile> Files { get; }
    IProject ConfigureOptions(Action<IProjectOptions> configureOptions);
}
