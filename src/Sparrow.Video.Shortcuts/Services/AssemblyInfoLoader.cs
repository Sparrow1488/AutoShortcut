using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Sparrow.Video.Shortcuts.Services;

public sealed class AssemblyInfoLoader
{
    public AssemblyInfoLoader(
        IConfiguration configuration,
        ILogger<AssemblyInfoLoader> logger)
    {
        Configuration = configuration;
        Logger = logger;
    }

    public IConfiguration Configuration { get; }
    public ILogger<AssemblyInfoLoader> Logger { get; }

    public string GetAssemblyInfo()
    {
        var assemblySection = Configuration.GetRequiredSection("Assembly");
        var name = assemblySection["Name"];
        var author = assemblySection["Author"];
        var version = assemblySection["Version"];
        return $"{name} version is {version}. Authored by {author}";
    }

    public string GetCurrentEnvironmentMode()
    {
        return Configuration.GetRequiredSection("Environment")["Mode"];
    }
}
