using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Services;

public class ProjectSerializationService : IProjectSerializationService
{
    private readonly IJsonSerializer _serializer;
    private readonly IPathsProvider _pathsProvider;
    private readonly ISaveService _saveService;
    private readonly ISharedProject _sharedProject;

    public ProjectSerializationService(
        IJsonSerializer serializer,
        IPathsProvider pathsProvider,
        ISaveService saveService,
        ISharedProject sharedProject)
    {
        _serializer = serializer;
        _pathsProvider = pathsProvider;
        _saveService = saveService;
        _sharedProject = sharedProject;
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
            var saveProjectOptionsPath = _pathsProvider.GetPathFromSharedProject(ProjectConfigSections.ProjectOptions);
            var saveSettings = new SaveSettings()
            {
                SaveFullPath = Path.Combine(saveProjectOptionsPath, "project-options.json")
            };
            await _saveService.SaveTextAsync(serializedOptions, saveSettings);
        });
    }

    private async Task OnEnableExecuteAsync(Func<Task> function)
    {
        if (_sharedProject.Project.Options.IsSerialize)
        {
            await function?.Invoke();
        }
    }
}
