﻿using Sparrow.Video.Abstractions.Projects.Options;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Projects.Options;

namespace Sparrow.Video.Shortcuts.Services;

public class RestoreProjectOptionsService : IRestoreProjectOptionsService
{
    private readonly IPathsProvider _pathsProvider;
    private readonly IJsonSerializer _serializer;
    private readonly IReadFileTextService _readFileTextService;

    public RestoreProjectOptionsService(
        IPathsProvider pathsProvider,
        IJsonSerializer serializer,
        IReadFileTextService readFileTextService)
    {
        _pathsProvider = pathsProvider;
        _serializer = serializer;
        _readFileTextService = readFileTextService;
    }

    public async Task<IProjectOptions> RestoreOptionsAsync()
    {
        var projectOptionsPath = _pathsProvider.GetPathFromCurrent("ProjectOptions");
        var fullPathToOptions = Path.Combine(projectOptionsPath, "project-options.json");
        var serializedOptions = await _readFileTextService.ReadTextAsync(fullPathToOptions);
        var options = _serializer.Deserialize<ProjectOptions>(serializedOptions);
        return options;
    }
}
