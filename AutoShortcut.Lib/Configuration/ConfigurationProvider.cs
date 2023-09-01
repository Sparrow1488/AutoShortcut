using AutoShortcut.Lib.Contracts.Services;
using AutoShortcut.Lib.Exceptions;

namespace AutoShortcut.Lib.Configuration;

public class ConfigurationProvider : IConfigurationProvider
{
    private FFmpegConfig? _ffmpegConfig;
    private StorageConfig? _storageConfig;
    private ProjectConfig? _projectConfig;

    public FFmpegConfig GetFFmpegConfig() => _ffmpegConfig ?? throw new ConfigurationException($"Not set instance of config type {nameof(FFmpegConfig)}");
    public StorageConfig GetStorageConfig() => _storageConfig ?? throw new ConfigurationException($"Not set instance of config type {nameof(StorageConfig)}");
    public ProjectConfig GetProjectConfig() => _projectConfig ?? new ProjectConfig();

    public ConfigurationProvider AddFFmpegConfig(FFmpegConfig config) => Return(() => _ffmpegConfig = config);
    public ConfigurationProvider AddStorageConfig(StorageConfig config) => Return(() => _storageConfig = config);
    public ConfigurationProvider AddProjectConfig(ProjectConfig config) => Return(() => _projectConfig = config);

    private ConfigurationProvider Return(Action action)
    {
        action.Invoke();
        return this;
    }
}