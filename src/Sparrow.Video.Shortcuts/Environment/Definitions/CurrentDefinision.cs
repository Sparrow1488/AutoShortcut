using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Projects.Options;
using Sparrow.Video.Abstractions.Render;
using Sparrow.Video.Abstractions.Runtime;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Processes;
using Sparrow.Video.Shortcuts.Projects;
using Sparrow.Video.Shortcuts.Projects.Options;
using Sparrow.Video.Shortcuts.Render;
using Sparrow.Video.Shortcuts.Runtime;
using Sparrow.Video.Shortcuts.Services;

namespace Sparrow.Video.Shortcuts.Environment.Definitions;

public class CurrentDefinision : ApplicationDefinition
{
    public override IServiceCollection OnConfigureServices(IServiceCollection services)
    {
        var projectConfiguration = new ConfigurationBuilder().AddJsonFile("appsettings.AutoShortcut.json").Build();
        services.AddScoped<IConfiguration>(x => projectConfiguration);

        services.AddSingleton<ISharedProject, SharedProject>();
        services.AddSingleton<IRuntimeProjectLoader, RuntimeProjectLoader>();

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
        services.AddSingleton<IProjectFileCreator, ProjectFileCreator>();
        services.AddSingleton<IProjectCreator, ShortcutProjectCreator>();
        services.AddSingleton<IProjectSaveSettingsCreator, ProjectSaveSettingsCreator>();
        services.AddSingleton<ITextFormatter, TextFormatter>();
        services.AddSingleton<AssemblyInfoLoader>();

        services.AddSingleton<IDefaultSaveService, DefaultSaveService>();

        var saveServiceSection = projectConfiguration.GetRequiredSection("Environment:Services:SaveService");
        var protectData = saveServiceSection["ProtectData"];
        if (protectData == ProtectDataTypes.Aes256)
        {
            services.AddSingleton<ISaveService, CryptoSaveService>();
            services.AddSingleton<IReadFileTextService, ReadEncryptedTextFilesService>();
        }
        if (protectData == ProtectDataTypes.None)
        {
            services.AddSingleton<ISaveService, SaveService>();
            services.AddSingleton<IReadFileTextService, ReadFileTextService>();
        }

        services.AddScoped<IAnalyseProcess, AnalyseProcess>();
        services.AddScoped<IConcatinateProcess, ConcatinateProcess>();
        services.AddScoped<IFFmpegProcess, DefaultFFmpegProjectProcess>();

        services.AddScoped<IProjectOptions, ProjectOptions>();

        services.AddScoped<IProjectSerializationService, ProjectSerializationService>();
        services.AddScoped<IRenderUtility, RenderUtility>();

        services.AddScoped<ICryptoService, AesCryptoService>();

        return services;
    }
}
