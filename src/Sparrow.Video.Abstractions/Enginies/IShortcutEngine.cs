using Sparrow.Video.Abstractions.Pipelines;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;

namespace Sparrow.Video.Abstractions.Enginies
{
    public interface IShortcutEngine
    {
        Task<IShortcutPipeline> CreatePipelineAsync(
            string filesDirectory, CancellationToken cancellationToken = default);
        Task<IFile> StartRenderAsync(
            IProject project, CancellationToken cancellationToken = default);
    }
}
