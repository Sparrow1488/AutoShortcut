using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sparrow.Video.Abstractions.Enginies;
using Sparrow.Video.Abstractions.Factories;
using Sparrow.Video.Shortcuts.Services;

namespace Sparrow.Video.Shortcuts.Factories;

public class ShortcutEngineFactory : IShortcutEngineFactory
{
    public ShortcutEngineFactory(
        ILogger<ShortcutEngineFactory> logger,
        IServiceProvider services,
        AssemblyInfoLoader assemblyInfoLoader)
    {
        _logger = logger;
        Services = services;
        AssemblyInfoLoader = assemblyInfoLoader;
    }

    private readonly ILogger<ShortcutEngineFactory> _logger;

    public IServiceProvider Services { get; private set; }
    public AssemblyInfoLoader AssemblyInfoLoader { get; }

    public IShortcutEngine CreateEngine()
    {
        _logger.LogInformation(AssemblyInfoLoader.GetAssemblyInfo());
        _logger.LogInformation("Current environment mode {mode}", AssemblyInfoLoader.GetCurrentEnvironmentMode());
        var engine = Services.GetService<IShortcutEngine>();
        return engine;
    }
}
