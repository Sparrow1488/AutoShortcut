using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Services.Options;

namespace Sparrow.Video.Abstractions.Services
{
    public interface IUploadFilesService
    {
        Task<ICollection<IFile>> GetFilesAsync(string path, CancellationToken token = default);
        ICollection<IFile> GetFiles(string path);
        ICollection<IFile> GetFiles(string path, IUploadFilesOptions uploadFilesOptions);
        IFile GetFile(string filePath);
    }
}
