using Sparrow.Video.Abstractions.Projects;

namespace Sparrow.Video.Abstractions.Services;

public interface IRestoreProjectService
{
    Task<IProject> RestoreAsync(
        string filesDirectoryPath, CancellationToken cancellationToken = default);

    Task<IProject> RestoreExistsAsync(
        string projectPath, CancellationToken cancellationToken = default);
}