using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects.Options;

namespace Sparrow.Video.Abstractions.Projects;

public interface IProjectCreator
{
    IProject CreateProjectWithOptions(IEnumerable<IProjectFile> files, IProjectOptions options);
    IProject CreateProject(IEnumerable<IProjectFile> files, Action<IProjectOptions> options);
    IProject CreateProject(IEnumerable<IProjectFile> files);
}
