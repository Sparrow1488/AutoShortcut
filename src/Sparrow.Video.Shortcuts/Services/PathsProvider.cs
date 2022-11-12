using Microsoft.Extensions.Configuration;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Services;

namespace Sparrow.Video.Shortcuts.Services;

public class PathsProvider : IPathsProvider
{
    public PathsProvider(
        IConfiguration configration,
        ISharedProject sharedProject)
    {
        Configration = configration;
        SharedProject = sharedProject;
    }

    public IConfiguration Configration { get; }
    public ISharedProject SharedProject { get; }

    public string GetPathFromCurrent(string name)
    {
        var currentDirPath = Directory.GetCurrentDirectory();
        var pathValue = GetPath(name);
        return Path.Combine(currentDirPath, pathValue);
    }

    public string GetPathFromProjectRoot(string name, IProjectRoot projectRoot)
    {
        var pathValue = GetPath(name);
        return Path.Combine(projectRoot.ProjectPaths.GetRoot(), pathValue);
    }

    public string GetPathFromSharedProject(string name)
    {
        var pathValue = GetPath(name);
        SharedProject.Assert();
        return Path.Combine(SharedProject.Project.Options.Root.ProjectPaths.GetRoot(), pathValue);
    }

    public string GetPath(string name)
    {
        var pathsSection = Configration.GetRequiredSection("Environment:Paths");
        var pathValue = pathsSection.GetValue<string>(name);
        return pathValue;
    }
}
