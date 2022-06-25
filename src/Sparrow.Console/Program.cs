using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Processes;
using Sparrow.Video.Shortcuts.Processes.Settings;
using Sparrow.Video.Shortcuts.Services;

var services = Host.CreateDefaultBuilder()
                   .ConfigureServices(services =>
{
    services.AddSingleton<IFileTypesProvider, FileTypesProvider>();
    services.AddSingleton<IUploadFilesService, UploadFilesService>();
    services.AddSingleton<IJsonFileAnalyseService, JsonAnalyseService>();
    services.AddSingleton<IAnalyseProcess, AnalyseProcess>();
    services.AddSingleton<IEncodingProcess, EncodingProcess>();
}).Build().Services;

string filesDirectory = @"D:\Йога\SFM\отдельно sfm\30";
var uploadService = services.GetRequiredService<IUploadFilesService>();
var filesCollection = uploadService.GetFiles(filesDirectory);

//var analyseProcess = services.GetRequiredService<IAnalyseProcess>();
//var analyse = await analyseProcess.GetAnalyseAsync(filesCollection.First());

var encodingProcess = services.GetRequiredService<IEncodingProcess>();
var settings = new EncodingSettings()
{
    EncodingType = EncodingType.Mpegts,
    SaveSettings = new SaveSettings() { SaveFullPath = @"C:\Users\USER\Downloads\new.ts" }
};
var encodedFile = await encodingProcess.StartEncodingAsync(filesCollection.First(), settings);

Console.ReadKey();