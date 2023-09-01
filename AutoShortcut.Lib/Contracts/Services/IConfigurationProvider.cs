using AutoShortcut.Lib.Configuration;

namespace AutoShortcut.Lib.Contracts.Services;

public interface IConfigurationProvider
{
    FFmpegConfig GetFFmpegConfig();
    StorageConfig GetStorageConfig();
    ProjectConfig GetProjectConfig();
}