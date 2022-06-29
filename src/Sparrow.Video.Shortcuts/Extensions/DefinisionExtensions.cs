using Microsoft.Extensions.DependencyInjection;
using Sparrow.Video.Shortcuts.Environment.Definitions;

namespace Sparrow.Video.Shortcuts.Extensions
{
    public static class DefinisionExtensions
    {
        static DefinisionExtensions()
        {
            DefaultDefinision = new CurrentDefinision();
        }

        public static ApplicationDefinition DefaultDefinision { get; }

        public static IServiceCollection AddShortcutDefinision(this IServiceCollection services)
        {
            return DefaultDefinision.OnConfigureServices(services);
        }

        public static IServiceCollection AddShortcutDefinision(
            this IServiceCollection services, ApplicationDefinition customDefinition)
        {
            return customDefinition.OnConfigureServices(services);
        }
    }
}
