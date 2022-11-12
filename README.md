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

Get from services `IRuntimeProjectLoader` and load new or exists project.

```C#
var loader = ServiceProvider.GetRequiredService<IRuntimeProjectLoader>();
loader.LoadEmpty();
//
const string savedProjectPath = "./test";
loader.LoadAsync(savedProjectPath);

loader.ConfigureProjectOptions(options =>
{
    options.Named(Variables.OutputFileName());
    options.StructureBy(new GroupStructure()
                            .StructureFilesBy(new NameStructure()));
    options.WithRules(container =>
    {
        container.AddRule<ScaleFileRule>();
        container.AddRule<SnapshotsFileRule>();
        container.AddRule<SilentFileRule>();
        container.AddRule<EncodingFileRule>();
        container.AddRule<LoopShortFileRule>();
        container.AddRule<LoopMediumFileRule>();
    });
    options.SetRootDirectory(saveProjectPath);
    options.Serialize(true);
});

var project = loader.CreateProject();
```

You can can choose not to use certain rules if you want VERY fast merge videos. Also apply files structure when finally project will created. This will allow you to structure your files logically in the finished video.

```C#
var renderUtility = ServiceProvider.GetRequiredService<IRenderUtility>();
var compilation = await renderUtility.StartRenderAsync(project);
Log.Information("Finally video: " + compilation.Path);
```

### CLI Usage

Example:

```powershell
./Sparrow.Console.exe input="./Files" output="Compilation" quality=fhd mode=new project-name="Compilation-Project" project-path="./" serialize=true
```



### Dependencies

* Newtonsoft.Json → **13.0.1**
* Microsoft.Extensions.Hosting → **6.0.1**
* Microsoft.Extensions.Configuration.Binder → **6.0.0**
* Microsoft.Extensions.Logging.Abstractions → **6.0.1**
* Microsoft.Extensions.Configuration.Abstractions → **6.0.0**
* Microsoft.Extensions.DependencyInjection.Abstractions → **6.0.0**
