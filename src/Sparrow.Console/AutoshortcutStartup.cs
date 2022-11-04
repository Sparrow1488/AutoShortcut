using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Sparrow.Console.Rules;
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
        Variables.SetVariable(EnvironmentVariableNames.Serialize, "true")
                 .SetVariable(EnvironmentVariableNames.OutputName, "Compilation")
                 .SetVariable(EnvironmentVariableNames.ProjectOpenMode, ProjectModes.New);
    }

    public override async Task OnStart(CancellationToken cancellationToken = default)
    {
        Log.Information(">> Starting {library}", "Autoshortcut");

        var logger = ServiceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<Startup>>();
        var factory = ServiceProvider.GetRequiredService<IShortcutEngineFactory>();
        var engine = factory.CreateEngine();

        Log.Information("Project Mode '{mode}'", Variables.CurrentProjectOpenMode());
        Log.Information("Get files from '{path}'", FilesDirectoryPath.Value);

        if (Variables.CurrentProjectOpenMode() == ProjectModes.Restore)
        {
            var restoredCompilation = await engine.ContinueRenderAsync(FilesDirectoryPath.Value, cancellationToken);
            return;
        }

        if (Variables.CurrentProjectOpenMode() == ProjectModes.New)
        {
            var pipeline = await engine.CreatePipelineAsync(
                        FilesDirectoryPath.Value, cancellationToken);

            Log.Information(
                "Project will serialize {isSerialize} and named {name}", 
                Variables.IsSerialize(),
                Variables.OutputFileName());

            var project = pipeline.Configure(options =>
            {
                options.IsSerialize = Variables.IsSerialize();
                options.AddRule<ScaleFileRule>();
                options.AddRule<SilentFileRule>();
                options.AddRule<EncodingFileRule>();
                options.AddRule<LoopShortFileRule>();
                options.AddRule<LoopMediumFileRule>();

            }).CreateProject(options =>
            {
                options.StructureBy(new GroupStructure(logger).StructureFilesBy(new NameStructure()));
                options.Named(Variables.OutputFileName());
            });

            var compilation = await engine.StartRenderAsync(project, cancellationToken);
            Log.Information("Finally video: " + compilation.Path);
            return;
        }

        throw new InvalidEnvironmentVariableException(
            $"Invalid input variable '{EnvironmentVariableNames.ProjectOpenMode}' is invalid");
    }
}
