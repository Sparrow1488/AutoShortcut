using Sparrow.Console.Abstractions;
using Sparrow.Console.Rules;
using Sparrow.Video.Abstractions.Enginies;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Runtime;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Primitives.Structures;

namespace Sparrow.Console;

internal class AutoshortcutStartup : AutoshortcutStartupBase
{
    public override void OnConfigreDevelopmentVariables(IEnvironmentVariablesProvider variables)
    {
        base.OnConfigreDevelopmentVariables(variables);
        Variables.SetVariable(EnvironmentVariableNames.InputDirectoryPath,
                             @"C:\Users\USER\Desktop\Test\Test2");
    }

    public override Task<IProject> OnRestoreProjectAsync(IRuntimeProjectLoader loader)
    {
        loader.ConfigureProjectOptions(options =>
        {
            options.Named("Loaded Compilation 2");
            options.StructureBy(new DurationStructure().LongFirst());
            // TODO:
            // 1. Сделать удобную настройку контейнера с правилами (не копировать все, а реплейсить конкретное)
            // ~  2. При установке правил изменить проверку с rules.Any() => set, на проверку каждого правила
            // 3. Рантайм лоадера можно использовать в engine.CreateProject, либо избавиться от этого метода и юзать отдельно рантаймера
            // +  4. AutoshortcutStartupBase

            options.WithRules(container =>
            {
                container.Replace<ScaleFileRule>(new(Resolution.Preview));
            });
        });
        var project = loader.CreateProject();
        return Task.FromResult(project);
    }

    public override async Task<IProject> OnCreateProjectAsync(
        IShortcutEngine engine, IEnumerable<IFile> files, string projectPath)
    {
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
            options.SetRootDirectory(projectPath);
        }, files, CancellationToken);

        return project;
    }
}
