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
    services.AddSingleton<IResourcesService, ResourcesService>();
    services.AddSingleton<IAnalyseProcess, AnalyseProcess>();
    services.AddSingleton<IEncodingProcess, EncodingProcess>();
    services.AddSingleton<IMakeSilentProcess, MakeSilentProcess>();
    services.AddSingleton<IFormatorProcess, VideoFormatorProcess>();
}).Build().Services;

string filesDirectory = @"D:\Йога\SFM\отдельно sfm\55";
var uploadService = services.GetRequiredService<IUploadFilesService>();
var filesCollection = uploadService.GetFiles(filesDirectory);

#region While not using
//var analyseProcess = services.GetRequiredService<IAnalyseProcess>();
//var analyse = await analyseProcess.GetAnalyseAsync(filesCollection.First());

//var encodingProcess = services.GetRequiredService<IEncodingProcess>();
//var settings = new EncodingSettings()
//{
//    EncodingType = EncodingType.Mpegts,
//    SaveSettings = new SaveSettings() { SaveFullPath = @$"C:\Users\USER\Downloads\{Path.GetRandomFileName()}.ts" }
//};
//var encodedFile = await encodingProcess.StartEncodingAsync(filesCollection.First(), settings);

//var silentProcess = services.GetRequiredService<IMakeSilentProcess>();
//var toSilentFile = filesCollection.Last();
//var saveSettings = new SaveSettings()
//{
//    SaveFullPath = @$"C:\Users\USER\Downloads\{toSilentFile.Name}{toSilentFile.Extension}"
//};
//var silentFile = await silentProcess.MakeSilentAsync(toSilentFile, saveSettings);

#endregion

var resourcesService = services.GetRequiredService<IResourcesService>();
var resource = resourcesService.GetRequiredResource("Resources:backgrounds:black:files:480p");

var formatorProcess = services.GetRequiredService<IFormatorProcess>();
var analyseProcess = services.GetRequiredService<IAnalyseProcess>();
var toFormatFile = filesCollection.First();
var analyse = await analyseProcess.GetAnalyseAsync(toFormatFile);
var settings = new VideoFormatSettings()
{
    DisplayResolution = Resolution.HighFHD,
    SaveSettings = new SaveSettings()
    {
        SaveFullPath = @$"C:\Users\USER\Downloads\{Path.GetRandomFileName()}.mp4"
    }
};
var formatterFile = await formatorProcess.CreateInFormatAsync(toFormatFile, analyse, settings);

Console.ReadKey();