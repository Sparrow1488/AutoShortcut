using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Services;

public class ProjectSaveSettingsCreator : IProjectSaveSettingsCreator
{
    private readonly IPathsProvider _pathsProvider;

    public ProjectSaveSettingsCreator(
        IPathsProvider pathsProvider)
    {
        _pathsProvider = pathsProvider;
    }

    public ISaveSettings Create(string sectionName, string fileName)
    {
        var projectSectionPath = _pathsProvider.GetPathFromSharedProject(sectionName);
        var projectFilePath = Path.Combine(projectSectionPath, fileName);
        return SaveSettings.Create(projectFilePath);
    }
}
