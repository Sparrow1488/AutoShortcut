using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Projects.Options;
using Sparrow.Video.Abstractions.Runtime;
using Sparrow.Video.Abstractions.Services;
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

    public async Task AddFileAsync(IFile file, CancellationToken cancellationToken = default)
    {
        if(!_projectFiles.Any(x => x.File.Path == file.Path))
        {
            var projectFile = await _fileCreator.CreateAsync(file, cancellationToken);
            _projectFiles.Add(projectFile);
        }
    }

    public void ConfigureProjectOptions(Action<IProjectOptions> options)
    {
        options?.Invoke(_projectOptions);
    }

    public IProject CreateProject()
    {
        if (_projectFiles is null || _projectFiles.Count == 0)
            throw new Exception("Files not loaded");
        if (_projectOptions is null)
            throw new Exception("Options not loaded");

        _projectOptions.ProjectFilesPaths = _projectFiles.Select(x => x.File.Path).ToArray();
        return _projectCreator.CreateProjectWithOptions(_projectFiles, _projectOptions);
    }

    public async Task LoadAsync(string projectPath)
    {
        var optionsAbsolutePath = _pathsProvider.GetPath("ProjectOptions");
        var optionsFullPath = Path.Combine(projectPath, optionsAbsolutePath);

        _projectOptions = (ProjectOptions)await _restoreOptionsService.RestoreOptionsAsync(optionsFullPath);
        var files = await _restoreFilesService.RestoreFilesAsync(_projectOptions.ProjectFilesPaths);
        _projectFiles = files.Select(x => x.RestoredProjectFile).ToList();
    }
}
