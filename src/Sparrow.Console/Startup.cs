using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Sparrow.Console.Rules;
using Sparrow.Video.Abstractions.Factories;
using Sparrow.Video.Primitives;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Primitives.Structures;

namespace Sparrow.Console
{
    internal class Startup
    {
        public Startup()
        {
            string filesDirectory = @"D:\Йога\SFM\отдельно sfm\NEW";
            FilesDirectoryPath = StringPath.CreateExists(filesDirectory);
        }

        private StringPath FilesDirectoryPath { get; }

        public IServiceProvider ServiceProvider { get; set; } = default!;

        public async Task OnStart(CancellationToken cancellationToken = default)
        {
            OnConfigureHost();
            var factory = ServiceProvider.GetRequiredService<IShortcutEngineFactory>();
            var engine = factory.CreateEngine();
            var pipeline = await engine.CreatePipelineAsync(
                            FilesDirectoryPath.Value, cancellationToken);

            var project = pipeline.Configure(options =>
            {
                options.IsSerialize = false;
                options.Rules.Add(ApplicationFileRules.ScaleFileRule);
                options.Rules.Add(ApplicationFileRules.SilentFileRule);
                options.Rules.Add(ApplicationFileRules.EncodingFileRule);
                options.Rules.Add(ApplicationFileRules.LoopMediumFileRule);
                options.Rules.Add(ApplicationFileRules.LoopShortFileRule);
            }).CreateProject(opt => opt.StructureBy(
                new GroupStructure().StructureFilesBy(new DurationStructure())));

            var compilation = await engine.StartRenderAsync(project, cancellationToken);
        }

        private void OnConfigureHost()
        {
            ServiceProvider = Host.CreateDefaultBuilder()
                .UseSerilog((context, services, configuration) => configuration
                    .Enrich.FromLogContext()
                    .WriteTo.Console())
                .ConfigureServices(services =>
                {
                    services.AddShortcutDefinision();
                })
                .Build().Services;
        }
    }
}
