using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Sparrow.Console.Rules;
using Sparrow.Video.Abstractions.Factories;
using Sparrow.Video.Primitives;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Primitives.Structures;

namespace Sparrow.Console;
internal class Startup
{
    public Startup()
    {
        string filesDirectory = @"D:\Yoga\SourceFilmMakers\PUBLIC\Categories\Overwatch\a";
        FilesDirectoryPath = StringPath.CreateExists(filesDirectory);
    }

    private StringPath FilesDirectoryPath { get; }

    public IServiceProvider ServiceProvider { get; set; } = default!;

    private void FixNames()
    {
        var files = Directory.GetFiles(FilesDirectoryPath.Value);
        foreach (var filePath in files)
        {
            var newName = Path.GetFileName(filePath).Replace("'", "");
            var newPath = Path.Combine(Path.GetDirectoryName(filePath), newName);
            File.Move(filePath, newPath);
        }
    }

    public async Task OnStart(CancellationToken cancellationToken = default)
    {
        FixNames();
        OnConfigureHost();
        var logger = ServiceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<Startup>>();
        var factory = ServiceProvider.GetRequiredService<IShortcutEngineFactory>();
        var engine = factory.CreateEngine();

        //var restoredCompilation = await engine.ContinueRenderAsync(FilesDirectoryPath.Value, cancellationToken);

        var pipeline = await engine.CreatePipelineAsync(
                        FilesDirectoryPath.Value, cancellationToken);

        var project = pipeline.Configure(options =>
        {
            options.IsSerialize = true; // TODO: Not implemented: Project RESTORE
            options.AddRule<ScaleFileRule>();
            options.AddRule<SilentFileRule>();
            options.AddRule<EncodingFileRule>();
            options.AddRule<LoopShortFileRule>();
            options.AddRule<LoopMediumFileRule>();

        }).CreateProject(options =>
        {
            options.StructureBy(new GroupStructure().StructureFilesBy(new DurationStructure()));
            options.Named("Compilation");
        });

        var compilation = await engine.StartRenderAsync(project, cancellationToken);
        Log.Information("Finally video: " + compilation.Path);
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
