using Microsoft.Extensions.Configuration;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Services;

namespace Sparrow.Video.Shortcuts.Services;

public class ResourcesService : IResourcesService
{
    public ResourcesService(
        IUploadFilesService uploadFilesService,
        IConfiguration configuration)
    {
        _uploadFilesService = uploadFilesService;
        Configuration = configuration;
    }

    private readonly IUploadFilesService _uploadFilesService;

    public IConfiguration Configuration { get; }

    public IFile GetRequiredResource(string address)
    {
        var resourcePath = Configuration.GetRequiredSection(address)
            .Get<string>();
        return _uploadFilesService.GetFile(resourcePath);             
    }
}