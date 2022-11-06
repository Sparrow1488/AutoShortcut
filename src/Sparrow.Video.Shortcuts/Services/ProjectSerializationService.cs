using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Services;

public class ProjectSerializationService : IProjectSerializationService
{
    private readonly IJsonSerializer _serializer;
    private readonly IPathsProvider _pathsProvider;
    private readonly ISaveService _saveService;
    private readonly IEnvironmentVariablesProvider _variablesProvider;

    public ProjectSerializationService(
        IJsonSerializer serializer,
        IPathsProvider pathsProvider,
        ISaveService saveService,
        IEnvironmentVariablesProvider variablesProvider)
    {
        _serializer = serializer;
        _pathsProvider = pathsProvider;
        _saveService = saveService;
        _variablesProvider = variablesProvider;

        IsEnabled = _variablesProvider.IsSerialize();
    }

    public bool IsEnabled { get; }

    public async Task SaveProjectFileAsync(IProjectFile file)
    {
        await OnEnableExecuteAsync(async () =>
        {
            var saveSettings = new SaveSettings()
            {
                SaveFullPath = file.File.Path.ChangeFileExtension(setExtension: ".restore")
            };
            var serializedFile = _serializer.Serialize(file);
            await _saveService.SaveTextAsync(serializedFile, saveSettings);
        });
    }

    public async Task SaveProjectFilesAsync(IEnumerable<IProjectFile> files)
    {
        await OnEnableExecuteAsync(async () =>
        {
            foreach (var file in files)
                await SaveProjectFileAsync(file);
        });
    }

    public async Task SaveProjectOptionsAsync(IProject project)
    {
        await OnEnableExecuteAsync(async () =>
        {
            var serializedOptions = _serializer.Serialize(project.Options);
            var saveProjectOptionsPath = _pathsProvider.GetPathFromSharedProject("ProjectOptions");
            var saveSettings = new SaveSettings()
            {
                SaveFullPath = Path.Combine(saveProjectOptionsPath, "project-options.json")
            };
            await _saveService.SaveTextAsync(serializedOptions, saveSettings);
        });
    }

    private async Task OnEnableExecuteAsync(Func<Task> function)
    {
        if (IsEnabled)
        {
            await function?.Invoke();
        }
    }
}
