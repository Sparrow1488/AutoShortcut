using System.Runtime.CompilerServices;
using AutoShortcut.Lib.Analyse;
using AutoShortcut.Lib.Configuration;
using AutoShortcut.Lib.Contracts.Core;
using AutoShortcut.Lib.Contracts.Services;
using AutoShortcut.Lib.Core;
using AutoShortcut.Lib.Helpful;
using AutoShortcut.Lib.Media;
using AutoShortcut.Lib.Montage;
using AutoShortcut.Lib.Processing;
using AutoShortcut.Lib.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace AutoShortcut.Lib.Extensions;

public static class AutoShortcutExtensions
{
    public static IServiceCollection AddAutoShortcut(this IServiceCollection services, Action<AutoShortcutConfiguration> configure)
    {
        var autoShortcutConfig = new AutoShortcutConfiguration();
        configure.Invoke(autoShortcutConfig);
        
        services.AddScoped<CompilationStrategy, DemuxerStrategy>();
        services.AddSingleton<IFFmpegEngine, FFmpegScriptEngine>();
        services.AddSingleton<IScriptEngine<MediaScriptResult, MediaExecutionContext>, FFmpegScriptEngine>();
        services.AddSingleton<INameService, NameService>();
        services.AddSingleton<ISerializeService, SerializeService>();
        services.AddScoped<IProcessManager, ProcessManager>();
        services.AddScoped<IAnalyserHelper, FFmpegAnalyserHelper>();
        services.AddScoped<IAnalyser, Analyser>();
        services.AddScoped<IMediaManager, MediaManager>();
        services.AddScoped<ITrackCompiler, MediaTrackCompiler>();
        services.AddScoped<IConfigurationProvider, ConfigurationProvider>(_ 
            => new ConfigurationProvider()
                .AddFFmpegConfig(autoShortcutConfig.FFmpegConfig!)
                .AddStorageConfig(autoShortcutConfig.StorageConfig!)
                .AddProjectConfig(autoShortcutConfig.ProjectConfig!)
        );

        return services;
    }
}

public class AutoShortcutConfiguration
{
    internal FFmpegConfig? FFmpegConfig { get; private set; }
    internal StorageConfig? StorageConfig { get; private set; }
    internal ProjectConfig? ProjectConfig { get; private set; }
    
    public AutoShortcutConfiguration AddFFmpegConfig(FFmpegConfig config)
    {
        FFmpegConfig = config;
        return this;
    }

    public AutoShortcutConfiguration AddStorageConfig(StorageConfig config)
    {
        StorageConfig = config;
        return this;
    }
    
    public AutoShortcutConfiguration AddProjectConfig(ProjectConfig config)
    {
        ProjectConfig = config;
        return this;
    }
}