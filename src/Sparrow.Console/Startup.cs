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
        string filesDirectory = @"D:\Yoga\SFM\отдельно sfm\fap9";
        FilesDirectoryPath = StringPath.CreateExists(filesDirectory);
    }

    private StringPath FilesDirectoryPath { get; }

    public IServiceProvider ServiceProvider { get; set; } = default!;

    public async Task OnStart(CancellationToken cancellationToken = default)
    {
        OnConfigureHost();
        var logger = ServiceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<Startup>>();
        var factory = ServiceProvider.GetRequiredService<IShortcutEngineFactory>();
        var engine = factory.CreateEngine();

        //var restoredCompilation = await engine.ContinueRenderAsync(FilesDirectoryPath.Value, cancellationToken);

        #region Not Used
        //var uploadFilesService = ServiceProvider.GetRequiredService<IUploadFilesService>();
        //var creator = ServiceProvider.GetRequiredService<IProjectFileCreator>();
        //var serializer = ServiceProvider.GetRequiredService<IJsonSerializer>();
        //var saveService = ServiceProvider.GetRequiredService<ISaveService>();
        //var files = uploadFilesService.GetFiles(FilesDirectoryPath.Value, 
        //    new UploadFilesOptions()
        //    {
        //        OnUploadedIgnoreFile = file => UploadFileAction.Skip
        //    }
        //    .Ignore(FileType.Restore)
        //    .Ignore(FileType.Audio));
        //foreach (var file in files)
        //{
        //    var projectFile = await creator.CreateAsync(file);
        //    var json = serializer.Serialize(projectFile);
        //    await saveService.SaveTextAsync(json, new SaveSettings()
        //    {
        //        SaveFullPath = Path.Combine(Path.GetDirectoryName(file.Path), file.Name + ".restore")
        //    }, cancellationToken);
        //}

        //var restoreService = ServiceProvider.GetRequiredService<IRestoreFilesService>();
        //var restoredFiles = await restoreService.RestoreFilesAsync(FilesDirectoryPath.Value);
        #endregion

        var pipeline = await engine.CreatePipelineAsync(
                        FilesDirectoryPath.Value, cancellationToken);

        var project = pipeline.Configure(options =>
        {
            options.IsSerialize = true; // TODO: Not implemented: Project RESTORE
            options.AddRule<ScaleFileRule>();
            options.AddRule<SilentFileRule>();
            options.AddRule<EncodingFileRule>();
            options.AddRule<LoopFileRule>();

        }).CreateProject(options =>
        {
            options.StructureBy(new GroupStructure().StructureFilesBy(new DurationStructure()));
            options.Named("Ready-Compilation");
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
