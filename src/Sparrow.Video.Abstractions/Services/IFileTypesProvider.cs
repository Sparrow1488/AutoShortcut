using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Abstractions.Services
{
    public interface IFileTypesProvider
    {
        string GetFileType(string extensions);
        string GetFileTypeOrUndefined(string extensions);
    }
}
