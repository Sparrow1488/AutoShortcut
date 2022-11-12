using Microsoft.Extensions.DependencyInjection;
using Sparrow.Video.Shortcuts.Environment.Definitions;

namespace Sparrow.Video.Shortcuts.Extensions;

public static class DefinisionExtensions
{
    public static IServiceCollection AddShortcutDefinision(
        this IServiceCollection services, IServiceProvider serviceProvider)
    {
        var definision = ActivatorUtilities.CreateInstance<CurrentDefinision>(serviceProvider);
        return definision.OnConfigureServices(services);
    }

    public static IServiceCollection AddShortcutDefinision(
        this IServiceCollection services, ApplicationDefinition customDefinition)
    {
        return customDefinition.OnConfigureServices(services);
    }
}
