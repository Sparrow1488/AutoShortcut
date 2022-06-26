using Sparrow.Video.Abstractions.Pipelines;

namespace Sparrow.Video.Abstractions.Enginies
{
    public interface IShortcutEngine
    {
        Task<IShortcutPipeline> CreatePipelineAsync(
            string filesDirectory, CancellationToken cancellationToken = default);
    }
}
