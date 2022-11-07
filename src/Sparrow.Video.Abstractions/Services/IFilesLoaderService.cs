using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Abstractions.Services;

public interface IFilesLoaderService
{
    Task<IEnumerable<IFile>> LoadFilesAsync(string filesDirectoryPath, CancellationToken cancellationToken = default);
}
