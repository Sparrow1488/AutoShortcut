using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;

namespace Sparrow.Video.Abstractions.Enginies;

public interface IShortcutEngine
{
    Task<IFile> StartRenderAsync(
        IProject project, CancellationToken cancellationToken = default);
}
