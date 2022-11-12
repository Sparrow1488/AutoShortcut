using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Abstractions.Services
{
    public interface IRestoreFilesService
    {
        Task<ICollection<IRestoreFile>> RestoreFilesAsync(string restoreDirectoryPath);
    }
}
