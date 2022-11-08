using Microsoft.Extensions.DependencyInjection;
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

    public override async Task<IProject> OnRestoreProjectAsync(IRuntimeProjectLoader loader)
    {
        var uploadService = ServiceProvider.GetRequiredService<IUploadFilesService>();
        var file = uploadService.GetFile(@"C:\Users\USER\Desktop\Main\Downloads\animech_3_downloads\2\GuraNew_H_Animtaion456251427.mp4");
        await loader.AddFileAsync(file, CancellationToken);

        loader.ConfigureProjectOptions(options =>
        {
            options.Named("Loaded Compilation");
            options.StructureBy(new DurationStructure().LongFirst());
            // TODO:
            // + 1. Сделать удобную настройку контейнера с правилами (не копировать все, а реплейсить конкретное)
            // ~  2. При установке правил изменить проверку с rules.Any() => set, на проверку каждого правила
            // 3. Рантайм лоадера можно использовать в engine.CreateProject, либо избавиться от этого метода и юзать отдельно рантаймера
            // +  4. AutoshortcutStartupBase
            // 5. Совместимость файлов с разными проектами (тут нужно проработать .restore файлы, а точнее их имена)

            options.WithRules(container =>
            {
                container.Replace<ScaleFileRule>(new(Resolution.Preview)); // пока применяется только к новым файлам
                container.Replace<ScaleFileRule>(new(Resolution.Preview));
            });
        });
        var project = loader.CreateProject();
        return project;
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
