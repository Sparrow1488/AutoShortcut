using Sparrow.Video.Abstractions.Projects.Options;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Projects.Options;

namespace Sparrow.Video.Shortcuts.Services;

public class RestoreProjectOptionsService : IRestoreProjectOptionsService
{
    private readonly IJsonSerializer _serializer;
    private readonly IReadFileTextService _readFileTextService;

    public RestoreProjectOptionsService(
        IJsonSerializer serializer,
        IReadFileTextService readFileTextService)
    {
        _serializer = serializer;
        _readFileTextService = readFileTextService;
    }

    public async Task<IProjectOptions> RestoreOptionsAsync(string optionsFilePath)
    {
        var fullPathToOptions = Path.Combine(optionsFilePath, "project-options.json");
        var serializedOptions = await _readFileTextService.ReadTextAsync(fullPathToOptions);
        var options = _serializer.Deserialize<ProjectOptions>(serializedOptions);
        return options;
    }
}
