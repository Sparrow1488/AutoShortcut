using Sparrow.Video.Abstractions.Pipelines;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Projects.Options;

namespace Sparrow.Video.Abstractions.Enginies;

public interface IShortcutEngine
{
    Task<IProject> CreateProjectAsync(
        Action<IProjectOptions> options, 
        IEnumerable<IFile> files,
        CancellationToken cancellationToken = default);

    //Task<IShortcutPipeline> CreatePipelineAsync(
    //    string filesDirectory, CancellationToken cancellationToken = default);

    Task<IFile> StartRenderAsync(
        IProject project, CancellationToken cancellationToken = default);
}
