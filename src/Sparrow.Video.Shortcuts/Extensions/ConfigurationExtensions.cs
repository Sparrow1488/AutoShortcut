using Microsoft.Extensions.Configuration;

namespace Sparrow.Video.Shortcuts.Extensions
{
    public static class ConfigurationExtensions
    {
        public static bool IsDevelopment(this IConfiguration configuration)
        {
            return configuration.GetRequiredSection("Environment:Current")
                                .Get<string>() == "Development";
        }

        public static bool IsDebug(this IConfiguration configuration)
        {
            return configuration.GetRequiredSection("Environment:Mode")
                                .Get<string>() == "Debug";
        }
    }
}
