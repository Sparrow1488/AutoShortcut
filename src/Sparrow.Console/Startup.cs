using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Serilog;
using Sparrow.Console.Rules;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Factories;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Abstractions.Services.Options;
using Sparrow.Video.Primitives;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Primitives;
using Sparrow.Video.Shortcuts.Primitives.Mementos;
using Sparrow.Video.Shortcuts.Primitives.Structures;
using Sparrow.Video.Shortcuts.Processes.Settings;
using Sparrow.Video.Shortcuts.Services.Options;

namespace Sparrow.Console;
internal class Startup
{
    public Startup()
    {
        string filesDirectory = @"D:\Yoga\SFM\отдельно sfm\50\Test-Moved";
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



        var uploadFilesService = ServiceProvider.GetRequiredService<IUploadFilesService>();
        var creator = ServiceProvider.GetRequiredService<IProjectFileCreator>();
        var serializer = ServiceProvider.GetRequiredService<IJsonSerializer>();
        var saveService = ServiceProvider.GetRequiredService<ISaveService>();
        var files = uploadFilesService.GetFiles(FilesDirectoryPath.Value, 
            new UploadFilesOptions()
            {
                OnUploadedIgnoreFile = file => UploadFileAction.Skip
            }
            .Ignore(FileType.Restore)
            .Ignore(FileType.Audio));
        foreach (var file in files)
        {
            var projectFile = await creator.CreateAsync(file);
            var json = serializer.Serialize(projectFile);
            await saveService.SaveTextAsync(json, new SaveSettings()
            {
                SaveFullPath = Path.Combine(Path.GetDirectoryName(file.Path), file.Name + ".restore")
            }, cancellationToken);
        }

        var restoreService = ServiceProvider.GetRequiredService<IRestoreFilesService>();
        var restoredFiles = await restoreService.RestoreFilesAsync(FilesDirectoryPath.Value);





        var pipeline = await engine.CreatePipelineAsync(
                        FilesDirectoryPath.Value, cancellationToken);

        var project = pipeline.Configure(options =>
        {
            // 1. Проверить, есть ли в загруженной папке с видео .restore файлы. В них будет сериализованный IFile
            // 2. Убедиться, что все .restore файлы есть для каждого файла проекта
            // 3. Если нет, то Rules будут применены (которые так же были сериализованы)
            // 4. Если правила для файлов не совпадают с уже примененными, то восстановление будет не возможно (напр. было HD, а сейчас 2K)

            options.IsSerialize = false; // TODO: Not implemented: Project RESTORE
            options.Rules.Add(ApplicationFileRules.ScaleFileRule);
            options.Rules.Add(ApplicationFileRules.SilentFileRule);
            options.Rules.Add(ApplicationFileRules.EncodingFileRule);
            options.Rules.Add(ApplicationFileRules.LoopMediumFileRule);
            options.Rules.Add(ApplicationFileRules.LoopShortFileRule);
        }).CreateProject(opt => opt.StructureBy(
            new GroupStructure().StructureFilesBy(new DurationStructure())))
        .Named("Ready-Compilation");

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
