using Microsoft.Extensions.DependencyInjection;
using Sparrow.Video.Abstractions.Enginies;
using Sparrow.Video.Abstractions.Factories;
using Sparrow.Video.Abstractions.Pipelines.Options;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Enginies;
using Sparrow.Video.Shortcuts.Factories;
using Sparrow.Video.Shortcuts.Pipelines.Options;
using Sparrow.Video.Shortcuts.Processes;
using Sparrow.Video.Shortcuts.Render;
using Sparrow.Video.Shortcuts.Services;

namespace Sparrow.Video.Shortcuts.Environment.Definitions
{
    public class CurrentDefinision : ApplicationDefinition
    {
        public override IServiceCollection OnConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IFileTypesProvider, FileTypesProvider>();
            services.AddSingleton<IPathsProvider, PathsProvider>();
            services.AddSingleton<IRuleProcessorsProvider, RuleProcessorsProvider>();
            services.AddSingleton<IScriptFormatsProvider, ScriptFormatsProvider>();

            services.AddSingleton<IUploadFilesService, UploadFilesService>();
            services.AddSingleton<IJsonFileAnalyseService, JsonAnalyseService>();
            services.AddSingleton<IResourcesService, ResourcesService>();
            services.AddSingleton<ISaveService, SaveService>();
            services.AddSingleton<IStoreService, StoreService>();
            services.AddSingleton<IProjectFileCreator, ProjectFileCreator>();
            services.AddSingleton<ITextFormatter, TextFormatter>();

            services.AddSingleton<IAnalyseProcess, AnalyseProcess>();
            services.AddSingleton<IEncodingProcess, EncodingProcess>();
            services.AddSingleton<IMakeSilentProcess, MakeSilentProcess>();
            services.AddSingleton<IFormatorProcess, VideoFormatorProcess>();
            services.AddSingleton<IConcatinateProcess, ConcatinateProcess>();
            services.AddSingleton<IScaleProcess, ScaleProcess>();

            services.AddSingleton<IShortcutEngineFactory, ShortcutEngineFactory>();

            services.AddScoped<IPipelineOptions, PipelineOptions>();
            services.AddScoped<IShortcutEngine, ShortcutEngine>();

            services.AddScoped<IRenderUtility, RenderUtility>();

            return services;
        }
    }
}
