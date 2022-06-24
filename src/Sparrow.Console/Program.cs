using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Services;

var services = Host.CreateDefaultBuilder()
                   .ConfigureServices(services =>
{
    services.AddSingleton<IFileTypesProvider, FileTypesProvider>();
    services.AddSingleton<IUploadFilesService, UploadFilesService>();
}).Build().Services;

var typesProvider = services.GetRequiredService<IFileTypesProvider>();
var fileType = typesProvider.GetFileTypeOrUndefined(".mp4");

string filesDirectory = @"D:\Йога\SFM\отдельно sfm\30";
var uploadService = services.GetRequiredService<IUploadFilesService>();
var filesCollection = uploadService.GetFiles(filesDirectory);

Console.ReadKey();