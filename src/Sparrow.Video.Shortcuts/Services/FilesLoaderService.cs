using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Services;

namespace Sparrow.Video.Shortcuts.Services;

public class FilesLoaderService : IFilesLoaderService
{
    public Task<IEnumerable<IFile>> LoadFilesAsync(
        string filesDirectoryPath, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
