using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;

namespace Sparrow.Video.Abstractions.Runtime;

public interface IRuntimeProjectLoader
{
    IEnumerable<IProjectFile> ProjectFiles { get; }

    Task LoadAsync(string projectPath);
    IProject CreateProject();
    Task AddFileAsync(IFile file, CancellationToken cancellationToken = default);
}
