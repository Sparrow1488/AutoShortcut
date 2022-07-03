![AutoShortcut-icon](https://github.com/Sparrow1488/Sparrow.Video.Shortcuts/blob/master/files/AutoShortcut-icon.png?raw=true)

There is mini library helps you easily create shortcut video. It merges and pre-processes your video before completed. Uses the open-source library `FFmpeg` for all video operations. 

### Usage

Inject dependencies in your application before start works.

```C#
ServiceProvider = Host.CreateDefaultBuilder()
                .ConfigureServices(services => 
                    services.AddShortcutDefinision())
                .Build().Services;
```

Create `ShortcutEngine` using factory or get required service from host services

```C#
var factory = ServiceProvider.GetRequiredService<IShortcutEngineFactory>();
var engine = factory.CreateEngine();
// or
var engine = ServiceProvider.GetRequiredService<IShortcutEngine>();
```

Create process pipeline to configure specify shortcut rules (encoding, set format, resolution or anything)

```C#
var pipeline = await engine.CreatePipelineAsync(
                            FilesDirectoryPath.Value, cancellationToken);

var project = pipeline.Configure(options =>
{
    options.IsSerialize = false; // save files meta for fast restore that
    options.Rules.Add(ApplicationFileRules.ScaleFileRule); // custom rules
    options.Rules.Add(ApplicationFileRules.SilentFileRule);
    options.Rules.Add(ApplicationFileRules.EncodingFileRule);
    options.Rules.Add(ApplicationFileRules.LoopMediumFileRule);
    options.Rules.Add(ApplicationFileRules.LoopShortFileRule);
}).CreateProject(opt => opt.StructureBy(
    new GroupStructure(logger).StructureFilesBy(new DurationStructure())));
```

You can can choose not to use certain rules if you want VERY fast merge videos. Also apply files structure when finally project will created. This will allow you to structure your files logically in the finished video.

```C#
System.Console.WriteLine("Finally video: " + compilation.Path);
```
