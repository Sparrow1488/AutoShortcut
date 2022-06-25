using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Abstractions.Services
{
    public interface IUploadFilesService
    {
        Task<ICollection<IFile>> GetFilesAsync(string path, CancellationToken token = default);
        ICollection<IFile> GetFiles(string path);
        IFile GetFile(string filePath);
    }
}
