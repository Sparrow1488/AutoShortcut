using Sparrow.Video.Abstractions.Projects;

namespace Sparrow.Video.Abstractions.Services
{
    public interface IRestoreProjectService
    {
        Task<IProject> RestoreAsync(
            string restoreFilesDirectoryPath, CancellationToken cancellationToken = default);
    }
}
