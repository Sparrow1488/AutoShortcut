using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Sparrow.Console.Rules;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Factories;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Exceptions;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Primitives.Structures;

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

        const string projectRootDirectory = "./[AutoShortcuts]Projects/Compilation-1.ash";

        if (Variables.CurrentProjectOpenMode() == ProjectModes.Restore)
        {
            var restoreService = ServiceProvider.GetRequiredService<IRestoreProjectService>();
            var project = await restoreService.RestoreExistsAsync(projectRootDirectory, cancellationToken);
            var compilation = await engine.StartRenderAsync(project, cancellationToken);

            return;
        }

        if (Variables.CurrentProjectOpenMode() == ProjectModes.New)
        {
            var pipeline = await engine.CreatePipelineAsync(FilesDirectoryPath.Value, cancellationToken);

            Log.Information(
                "Project serialize {isSerialize}; Named: {name}; Resolution: {resolution}", 
                Variables.IsSerialize(),
                Variables.OutputFileName(),
                Variables.GetOutputVideoQuality());

            ScaleFileRule outputVideoResolutionScale 
                = new(Resolution.ParseRequiredResolution(Variables.GetOutputVideoQuality()));

            var project = pipeline
                .Configure(opt => opt.IsSerialize = Variables.IsSerialize())
                .CreateProject(options =>
                {
                    options.Named(Variables.OutputFileName());
                    options.StructureBy(new GroupStructure().StructureFilesBy(new NameStructure()));
                    options.WithRules(rulesContainer =>
                    {
                        rulesContainer.AddRule(outputVideoResolutionScale);
                        //rulesContainer.AddRule<SnapshotsFileRule>();
                        rulesContainer.AddRule<SilentFileRule>();
                        rulesContainer.AddRule<EncodingFileRule>();
                        rulesContainer.AddRule<LoopShortFileRule>();
                        rulesContainer.AddRule<LoopMediumFileRule>();
                    });
                    options.SetRootDirectory(projectRootDirectory);
                });

            var compilation = await engine.StartRenderAsync(project, cancellationToken);
            Log.Information("Finally video: " + compilation.Path);
            return;
        }

        throw new InvalidEnvironmentVariableException(
            $"Invalid input variable '{EnvironmentVariableNames.ProjectOpenMode}' is invalid");
    }
}
