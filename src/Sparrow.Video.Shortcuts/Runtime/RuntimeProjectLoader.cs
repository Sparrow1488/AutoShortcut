using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Projects.Options;
using Sparrow.Video.Abstractions.Runtime;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Projects.Options;

namespace Sparrow.Video.Shortcuts.Runtime;

public class RuntimeProjectLoader : IRuntimeProjectLoader
{
    private readonly IProjectCreator _projectCreator;
    private readonly IProjectFileCreator _fileCreator;
    private readonly IPathsProvider _pathsProvider;
    private readonly IRestoreFilesService _restoreFilesService;
    private readonly IRestoreProjectOptionsService _restoreOptionsService;

    private List<IProjectFile> _projectFiles = new();
    private ProjectOptions _projectOptions;

    public RuntimeProjectLoader(
        IPathsProvider pathsProvider,
        IProjectCreator projectCreator,
        IProjectFileCreator fileCreator,
        IRestoreFilesService restoreFilesService,
        IRestoreProjectOptionsService restoreOptionsService)
    {
        _projectCreator = projectCreator;
        _fileCreator = fileCreator;
        _pathsProvider = pathsProvider;
        _restoreFilesService = restoreFilesService;
        _restoreOptionsService = restoreOptionsService;
    }

    public IEnumerable<IProjectFile> ProjectFiles => _projectFiles;

    public async Task LoadAsync(string projectPath)
    {
        var optionsAbsolutePath = _pathsProvider.GetPath(ProjectConfigSections.ProjectOptions);
        var optionsFullPath = Path.Combine(projectPath, optionsAbsolutePath);

        var projectOptions = (ProjectOptions)await _restoreOptionsService.RestoreOptionsAsync(optionsFullPath);
        var files = await _restoreFilesService.RestoreFilesAsync(projectOptions.ProjectFilesPaths);
        var projectFiles = files.Select(x => x.RestoredProjectFile).ToList();
        InitProject(projectOptions, projectFiles);
    }

    public void LoadEmpty()
    {
        InitProject(options: new(), files: Array.Empty<IProjectFile>());
    }

    private void InitProject(ProjectOptions options, IEnumerable<IProjectFile> files)
    {
        _projectOptions = options;
        _projectFiles = files.ToList();
    }

    public Task AddFilesAsync(IEnumerable<IFile> files, CancellationToken cancellationToken = default)
    {
        return OnAssertedLoadExecuteAsync(async () =>
        {
            foreach (var file in files)
                await AddFileAsync(file, cancellationToken);
        });
    }

    public Task AddFileAsync(IFile file, CancellationToken cancellationToken = default)
    {
        return OnAssertedLoadExecuteAsync(async () =>
        {
            if (!_projectFiles.Any(x => x.File.Path == file.Path))
            {
                var projectFile = await _fileCreator.CreateAsync(file, cancellationToken);
                _projectFiles.Add(projectFile);
            }
        });
    }

    public void ConfigureProjectOptions(Action<IProjectOptions> options)
    {
        OnAssertedLoadExecute(() => options?.Invoke(_projectOptions));
    }

    public IProject CreateProject()
    {
        IProject project = default;
        OnAssertedLoadExecute(() =>
        {
            _projectOptions.ProjectFilesPaths = _projectFiles.Select(x => x.File.Path).ToArray();
            project = _projectCreator.CreateProjectWithOptions(_projectFiles, _projectOptions);
        });
        return project;
    }

    private void OnAssertedLoadExecute(Action action)
    {
        AssertLoaded();
        action?.Invoke();
    }

    private Task OnAssertedLoadExecuteAsync(Func<Task> action)
    {
        AssertLoaded();
        return action?.Invoke();
    }

    private void AssertLoaded()
    {
        if (_projectFiles is null)
            throw new Exception("Files not loaded");
        if (_projectOptions is null)
            throw new Exception("Options not loaded");
    }
}
