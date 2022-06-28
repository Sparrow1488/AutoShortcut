using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sparrow.Video.Abstractions.Enginies;
using Sparrow.Video.Abstractions.Factories;
using Sparrow.Video.Abstractions.Pipelines.Options;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processors;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Enginies;
using Sparrow.Video.Shortcuts.Pipelines.Options;
using Sparrow.Video.Shortcuts.Processes;
using Sparrow.Video.Shortcuts.Processors;
using Sparrow.Video.Shortcuts.Rules;
using Sparrow.Video.Shortcuts.Services;

namespace Sparrow.Video.Shortcuts.Factories
{
    public class ShortcutEngineFactory : IShortcutEngineFactory
    {
        public IServiceProvider Services { get; private set; }

        public IShortcutEngine CreateEngine()
        {
            if (Services is null)
            {
                ConfigureServices();
            }
            var engine = Services.GetService<IShortcutEngine>();
            return engine;
        }

        private void ConfigureServices()
        {
            Services = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
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

                    services.AddSingleton<IAnalyseProcess, AnalyseProcess>();
                    services.AddSingleton<IEncodingProcess, EncodingProcess>();
                    services.AddSingleton<IMakeSilentProcess, MakeSilentProcess>();
                    services.AddSingleton<IFormatorProcess, VideoFormatorProcess>();
                    services.AddSingleton<IConcatinateProcess, ConcatinateProcess>();

                    services.AddScoped<IPipelineOptions, PipelineOptions>();
                    services.AddScoped<IShortcutEngine, ShortcutEngine>();
                }).Build().Services;
        }
    }
}
