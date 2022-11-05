using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sparrow.Video.Abstractions.Enginies;
using Sparrow.Video.Abstractions.Factories;
using Sparrow.Video.Abstractions.Pipelines;
using Sparrow.Video.Abstractions.Pipelines.Options;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Projects.Options;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Enginies;
using Sparrow.Video.Shortcuts.Factories;
using Sparrow.Video.Shortcuts.Pipelines;
using Sparrow.Video.Shortcuts.Pipelines.Options;
using Sparrow.Video.Shortcuts.Processes;
using Sparrow.Video.Shortcuts.Projects;
using Sparrow.Video.Shortcuts.Projects.Options;
using Sparrow.Video.Shortcuts.Render;
using Sparrow.Video.Shortcuts.Services;

namespace Sparrow.Video.Shortcuts.Environment.Definitions;

public class CurrentDefinision : ApplicationDefinition
{
    public override IServiceCollection OnConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IConfiguration>(x => new ConfigurationBuilder().AddJsonFile("appsettings.AutoShortcut.json").Build());

        services.AddSingleton<IFileTypesProvider, FileTypesProvider>();
        services.AddSingleton<IEnvironmentVariablesProvider, EnvironmentVariablesProvider>();
        services.AddSingleton<IPathsProvider, PathsProvider>();
        services.AddSingleton<IRuleProcessorsProvider, RuleProcessorsProvider>();
        services.AddSingleton<IScriptFormatsProvider, ScriptFormatsProvider>();
        services.AddSingleton<IEnvironmentSettingsProvider, EnvironmentSettingsProvider>();
        services.AddSingleton<IJsonSerializer, JsonSerializer>();

        services.AddSingleton<IUploadFilesService, UploadFilesService>();
        services.AddSingleton<IJsonFileAnalyseService, JsonAnalyseService>();
        services.AddSingleton<IResourcesService, ResourcesService>();
        services.AddSingleton<IStoreService, StoreService>();
        services.AddSingleton<IRestoreFilesService, RestoreFilesService>();
        services.AddSingleton<IRestoreProjectOptionsService, RestoreProjectOptionsService>();
        services.AddSingleton<IRestoreProjectService, RestoreProjectService>();
        services.AddSingleton<IProjectFileCreator, ProjectFileCreator>();
        services.AddSingleton<IProjectCreator, ShortcutProjectCreator>();
        services.AddSingleton<ITextFormatter, TextFormatter>();
        services.AddSingleton<AssemblyInfoLoader>();

        services.AddSingleton<IDefaultSaveService, DefaultSaveService>();

        bool encryptSavingFiles = false;
        if (encryptSavingFiles)
        {
            services.AddSingleton<ISaveService, CryptoSaveService>();
            services.AddSingleton<IReadFileTextService, ReadEncryptedTextFilesService>();
        }
        else
        {
            services.AddSingleton<ISaveService, SaveService>();
            services.AddSingleton<IReadFileTextService, ReadFileTextService>();
        }

        services.AddSingleton<IAnalyseProcess, AnalyseProcess>();
        services.AddSingleton<IEncodingProcess, EncodingProcess>();
        services.AddSingleton<IMakeSilentProcess, MakeSilentProcess>();
        services.AddSingleton<IFormatorProcess, VideoFormatorProcess>();
        services.AddSingleton<IConcatinateProcess, ConcatinateProcess>();
        services.AddSingleton<IScaleProcess, ScaleProcess>();

        services.AddSingleton<IShortcutEngineFactory, ShortcutEngineFactory>();

        services.AddScoped<IPipelineOptions, PipelineOptions>();
        services.AddScoped<IProjectOptions, ProjectOptions>();
        services.AddScoped<IShortcutEngine, ShortcutEngine>();
        services.AddScoped<IShortcutPipeline, ShortcutPipeline>();

        services.AddScoped<IProjectSerializationService, ProjectSerializationService>();
        services.AddScoped<IRenderUtility, RenderUtility>();

        services.AddScoped<ICryptoService, AesCryptoService>();

        return services;
    }
}
