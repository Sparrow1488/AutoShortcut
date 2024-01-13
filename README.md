# AutoShortcut

## Usage

1. Зарегистрируйте внутренние серивисы:

```C#
var services = new ServiceCollection();

services.AddAutoShortcut(config =>
{
    config
        .AddFFmpegConfig(new FFmpegConfig(ffmpeg, ffprobe))
        .AddStorageConfig(new StorageConfig(tempPath, personalPath))
        .AddProjectConfig(new ProjectConfig("Example.mp4"));
});

var di = services.BuildServiceProvider();
```

2. Загрузите медиа, используя `IMediaManager`:

```c#
var mediaManager = di.GetRequiredService<IMediaManager>();

var mediaList = new List<IMediaFile>();
foreach (var pathFile in Directory.GetFiles(directory))
{
    mediaList.Add(await mediaManager.LoadAnalysedAsync(pathFile));
}
```

3. Создайте реализацию `ITrack` и вложите в него свои файлы:

```C#
var track = new MediaTrack(mediaList);
```

4. Наложите эффекты на медиа на ваше усмотрение. Пример:

```C#
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
```

5. Запустите компиляцию:

```C#
var compiler = di.GetRequiredService<ITrackCompiler>();
var result = await compiler.CompileAsync(track);
```

