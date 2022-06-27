using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sparrow.Video.Abstractions.Enginies;
using Sparrow.Video.Abstractions.Pipelines.Options;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Enginies;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Pipelines.Options;
using Sparrow.Video.Shortcuts.Processes;
using Sparrow.Video.Shortcuts.Rules;
using Sparrow.Video.Shortcuts.Services;

var services = Host.CreateDefaultBuilder()
                   .ConfigureServices(services =>
{
    services.AddSingleton<IFileTypesProvider, FileTypesProvider>();
    services.AddSingleton<IPathsProvider, PathsProvider>();

    services.AddSingleton<IUploadFilesService, UploadFilesService>();
    services.AddSingleton<IJsonFileAnalyseService, JsonAnalyseService>();
    services.AddSingleton<IResourcesService, ResourcesService>();
    services.AddSingleton<ISaveService, SaveService>();
    services.AddSingleton<IStoreService, StoreService>();

    services.AddSingleton<IAnalyseProcess, AnalyseProcess>();
    services.AddSingleton<IEncodingProcess, EncodingProcess>();
    services.AddSingleton<IMakeSilentProcess, MakeSilentProcess>();
    services.AddSingleton<IFormatorProcess, VideoFormatorProcess>();

    services.AddScoped<IPipelineOptions, PipelineOptions>();
    services.AddScoped<IShortcutEngine, ShortcutEngine>();
}).Build().Services;

string filesDirectory = @"D:\Йога\SFM\отдельно sfm\55";
var uploadService = services.GetRequiredService<IUploadFilesService>();
var filesCollection = uploadService.GetFiles(filesDirectory);

var engine = services.GetRequiredService<IShortcutEngine>();
var pipeline = await engine.CreatePipelineAsync(filesDirectory);
var project = pipeline.Configure(options =>
{
    options.Rules.Add(LoopFileRule.Default);
    options.Rules.Add(SilentAudioRule.Default); // БОЛЬШОЙ ПОТЕНЦИАЛ
    var groupName = "overwatch";
    options.Rules.Add(new GroupFileRule(groupName,
                        file => file.File.Name.Contains(groupName)));
}).CreateProject();

Console.ReadKey();