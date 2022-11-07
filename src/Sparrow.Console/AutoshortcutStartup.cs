using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Sparrow.Console.Rules;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Factories;
using Sparrow.Video.Abstractions.Runtime;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Exceptions;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Primitives.Structures;
using Sparrow.Video.Shortcuts.Services.Options;

namespace Sparrow.Console;

internal class AutoshortcutStartup : Startup
{
    public override void OnConfigreDevelopmentVariables(IEnvironmentVariablesProvider variables)
    {
        base.OnConfigreDevelopmentVariables(variables);
        Variables.SetVariable(EnvironmentVariableNames.InputDirectoryPath,
                             @"C:\Users\USER\Desktop\Test\Test2");
    }

    public override async Task OnStart(CancellationToken cancellationToken = default)
    {
        Log.Information(">> Starting {library}", "Autoshortcut");

        var logger = ServiceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<Startup>>();
        var factory = ServiceProvider.GetRequiredService<IShortcutEngineFactory>();
        var engine = factory.CreateEngine();

        Log.Information("Project Mode '{mode}'", Variables.CurrentProjectOpenMode());
        Log.Information("Get files from '{path}'", FilesDirectoryPath.Value);

        const string projectRootDirectory = "./[AutoShortcuts]Projects/Compilation-2.ash";

        if (Variables.CurrentProjectOpenMode() == ProjectModes.Restore)
        {
            var loader = ServiceProvider.GetRequiredService<IRuntimeProjectLoader>();
            await loader.LoadAsync(projectRootDirectory);

            var uploadService = ServiceProvider.GetRequiredService<IUploadFilesService>();
            var newFile = uploadService.GetFile(@"C:\Users\USER\Desktop\Test\Test\InaNew_H_Animation456251429.mp4");

            await loader.AddFileAsync(newFile, cancellationToken);
            var project = loader.CreateProject();

            var compilation = await engine.StartRenderAsync(project, cancellationToken);
            Log.Information("Finally video: " + compilation.Path);
            return;
        }

        if (Variables.CurrentProjectOpenMode() == ProjectModes.New)
        {
            Log.Information(
                "Project serialize {isSerialize}; Named: {name}; Resolution: {resolution}", 
                Variables.IsSerialize(),
                Variables.OutputFileName(),
                Variables.GetOutputVideoQuality());

            var uploadService = ServiceProvider.GetRequiredService<IUploadFilesService>();
            var files = await uploadService.GetFilesAsync(
                                FilesDirectoryPath.Value, 
                                GetUploadOptions(),
                                cancellationToken);

            var project = await engine.CreateProjectAsync(options =>
            {
                options.Named(Variables.OutputFileName());
                options.StructureBy(new GroupStructure().StructureFilesBy(new NameStructure()));
                options.WithRules(rulesContainer =>
                {
                    ScaleFileRule outputVideoResolutionScale = new(Resolution.ParseRequiredResolution(Variables.GetOutputVideoQuality()));

                    rulesContainer.AddRule(outputVideoResolutionScale);
                    rulesContainer.AddRule<SnapshotsFileRule>();
                    rulesContainer.AddRule<SilentFileRule>();
                    rulesContainer.AddRule<EncodingFileRule>();
                    rulesContainer.AddRule<LoopShortFileRule>();
                    rulesContainer.AddRule<LoopMediumFileRule>();
                });
                options.SetRootDirectory(projectRootDirectory);
            }, files, cancellationToken);

            var compilation = await engine.StartRenderAsync(project, cancellationToken);
            Log.Information("Finally video: " + compilation.Path);
            return;
        }

        throw new InvalidEnvironmentVariableException(
            $"Invalid input variable '{EnvironmentVariableNames.ProjectOpenMode}' is invalid");
    }

    private static UploadFilesOptions GetUploadOptions()
    {
        UploadFilesOptions options = new()
        {
            OnUploadedIgnoreFile = (file) => UploadFileAction.Skip
        };
        options.Ignore(FileType.Undefined)
               .Ignore(FileType.Restore)
               .Ignore(FileType.Image)
               .Ignore(FileType.Audio);
        return options;
    }
}
