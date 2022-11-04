using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;

namespace Sparrow.Video.Abstractions.Services;

public interface IProjectSerializationService
{
    bool IsEnabled { get; }
    Task SaveProjectOptionsAsync(IProject project);
    Task SaveProjectFilesAsync(IEnumerable<IProjectFile> files);
    Task SaveProjectFileAsync(IProjectFile file);
}
