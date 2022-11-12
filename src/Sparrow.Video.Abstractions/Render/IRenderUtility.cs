using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;

namespace Sparrow.Video.Abstractions.Render;

/// <summary>
///     Prepare and render files
/// </summary>
public interface IRenderUtility
{
    Task<IFile> StartRenderAsync(IProject project, CancellationToken cancellationToken = default);
}
