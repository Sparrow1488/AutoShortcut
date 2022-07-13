using Microsoft.Extensions.Configuration;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Enums;

namespace Sparrow.Video.Shortcuts.Services;
public class EnvironmentSettingsProvider : IEnvironmentSettingsProvider
{
    public EnvironmentSettingsProvider(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public bool IsFFmpegScriptsLoggingEnabled()
    {
        return Configuration.GetRequiredSection("Environment:Settings:FFmpegScriptsLogging").Get<string>()
             == EnvironmentSettings.FFmpegLogging.Enable;
    }
}
