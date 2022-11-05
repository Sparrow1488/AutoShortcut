using Microsoft.Extensions.Configuration;
using Sparrow.Video.Abstractions.Services;

namespace Sparrow.Video.Shortcuts.Services;

public class PathsProvider : IPathsProvider
{
    public PathsProvider(IConfiguration configration)
    {
        Configration = configration;
    }

    public IConfiguration Configration { get; }

    public string GetPath(string name)
    {
        var pathsSection = Configration.GetRequiredSection("Environment:Paths");
        var pathValue = pathsSection.GetValue<string>(name);
        return pathValue;
    }

    public string GetPathFromCurrent(string name)
    {
        var currentDirPath = Directory.GetCurrentDirectory();
        var pathValue = GetPath(name);
        return Path.Combine(currentDirPath, pathValue);
    }
}
