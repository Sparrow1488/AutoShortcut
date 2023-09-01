using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Abstractions.Services;

public interface IResourcesService
{
    IFile GetRequiredResource(string address);
}