using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Abstractions.Services
{
    public interface IProjectFileCreator
    {
        Task<IProjectFile> CreateAsync(IFile file, CancellationToken cancellationToken = default);
    }
}
