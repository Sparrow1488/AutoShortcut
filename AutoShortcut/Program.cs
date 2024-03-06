using AutoShortcut.Lib.Configuration;
using AutoShortcut.Lib.Contracts.Media;
using AutoShortcut.Lib.Contracts.Services;
using AutoShortcut.Lib.Extensions;
using AutoShortcut.Lib.Media;
using AutoShortcut.Lib.Montage;
using Microsoft.Extensions.DependencyInjection;

#region Constants

const string tempPath = "./temp";
const string personalPath = "./personal";
const string ffmpeg = @"C:\Users\ilyao\OneDrive\Documents\ffmpeg\bin\ffmpeg.exe";
const string ffprobe = @"C:\Users\ilyao\OneDrive\Documents\ffmpeg\bin\ffprobe.exe";
const string directory = @"C:\Users\ilyao\OneDrive\Desktop\Data";

#endregion

var services = new ServiceCollection();

#region Services

Directory.CreateDirectory(tempPath);
Directory.CreateDirectory(personalPath);

services.AddAutoShortcut(config =>
{
    config
        .AddFFmpegConfig(new FFmpegConfig(ffmpeg, ffprobe))
        .AddStorageConfig(new StorageConfig(tempPath, personalPath))
        .AddProjectConfig(new ProjectConfig("AutoShortcut_Result.mp4"));
});

#endregion

var di = services.BuildServiceProvider();

Console.WriteLine("Hello, World!");

var mediaManager = di.GetRequiredService<IMediaManager>();

var mediaList = new List<IMediaFile>();
foreach (var pathFile in Directory.GetFiles(directory))
{
    mediaList.Add(await mediaManager.LoadAnalysedAsync(pathFile));
}

var track = new MediaTrack(mediaList);

foreach (var media in track.Media)
{
    const int fadeSeconds = 1;

    media.Scale(new Resolution360Px(), di)
         .Loop(2, di)
         .FadeIn(fadeSeconds, di)
         .FadeOut(fadeSeconds, di);
}

var compiler = di.GetRequiredService<ITrackCompiler>();
var result = await compiler.CompileAsync(track);

Console.WriteLine("Result video: " + result.Path);