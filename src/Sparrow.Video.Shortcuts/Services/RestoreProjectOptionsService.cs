using Sparrow.Video.Abstractions.Projects.Options;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Projects.Options;

namespace Sparrow.Video.Shortcuts.Services;

public class RestoreProjectOptionsService : IRestoreProjectOptionsService
{
    private readonly IPathsProvider _pathsProvider;
    private readonly IJsonSerializer _serializer;
    private readonly IReadFileTextService _readFileTextService;
    private readonly IEnvironmentVariablesProvider _variablesProvider;

    public RestoreProjectOptionsService(
        IPathsProvider pathsProvider,
        IJsonSerializer serializer,
        IReadFileTextService readFileTextService,
        IEnvironmentVariablesProvider variablesProvider)
    {
        _pathsProvider = pathsProvider;
        _serializer = serializer;
        _readFileTextService = readFileTextService;
        _variablesProvider = variablesProvider;
    }

    public async Task<IProjectOptions> RestoreOptionsAsync()
    {
        var projectOptionsPath = _pathsProvider.GetPathFromSharedProject("ProjectOptions");
        var fullPathToOptions = Path.Combine(projectOptionsPath, "project-options.json");
        var serializedOptions = await _readFileTextService.ReadTextAsync(fullPathToOptions);
        var options = _serializer.Deserialize<ProjectOptions>(serializedOptions);
        return options;
    }
}
