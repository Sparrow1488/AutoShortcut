using AutoShortcut.Lib.Analyse;
using AutoShortcut.Lib.Configuration;
using AutoShortcut.Lib.Contracts.Core;
using AutoShortcut.Lib.Contracts.Media;
using AutoShortcut.Lib.Contracts.Services;
using AutoShortcut.Lib.Core;
using AutoShortcut.Lib.Helpful;
using AutoShortcut.Lib.Media;
using AutoShortcut.Lib.Montage;
using AutoShortcut.Lib.Montage.Effects;
using AutoShortcut.Lib.Processing;
using AutoShortcut.Lib.Strategies;
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

services.AddScoped<IConfigurationProvider, ConfigurationProvider>(_ 
    => new ConfigurationProvider()
        .AddFFmpegConfig(new FFmpegConfig(ffmpeg, ffprobe))
        .AddStorageConfig(new StorageConfig(tempPath, personalPath))
        .AddProjectConfig(new ProjectConfig("AutoShortcut_Result.mp4"))
);
services.AddScoped<CompilationStrategy, DemuxerStrategy>();
services.AddSingleton<IFFmpegEngine, FFmpegScriptEngine>();
services.AddSingleton<IScriptEngine<MediaScriptResult, MediaExecutionContext>, FFmpegScriptEngine>();
services.AddSingleton<INameService, NameService>();
services.AddSingleton<ISerializeService, SerializeService>();
services.AddScoped<IProcessManager, ProcessManager>();
services.AddScoped<IAnalyserHelper, FFmpegAnalyserHelper>();
services.AddScoped<IAnalyser, Analyser>();
services.AddScoped<IMediaManager, MediaManager>();
services.AddScoped<ITrackCompiler, MediaTrackCompiler>();

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
    
    media.AddEffect(new ScaleEffect(new Resolution360Px(), di))
         .AddEffect(new LoopVideo(2, di))
         .AddEffect(new FadeInEffect(0, fadeSeconds, di))
         .AddEffect(previous 
             => new FadeOutEffect((int) previous!.MediaFormat!.Duration - fadeSeconds, fadeSeconds, di)
         );
}

var compiler = di.GetRequiredService<ITrackCompiler>();
var result = await compiler.CompileAsync(track);

Console.WriteLine("Result video: " + result.Path);