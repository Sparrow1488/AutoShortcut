﻿using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Sparrow.Video.Abstractions.Enginies;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Factories;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Runtime;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Exceptions;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Services.Options;

namespace Sparrow.Console.Abstractions;

internal abstract class AutoshortcutStartupBase : Startup
{
    public CancellationToken CancellationToken { get; private set; }

    public abstract Task<IProject> OnRestoreProjectAsync(IRuntimeProjectLoader loader);
    public abstract Task<IProject> OnCreateProjectAsync(
        IShortcutEngine engine, IEnumerable<IFile> files, string projectPath);

    public override async Task OnStart(CancellationToken cancellationToken = default)
    {
        CancellationToken = cancellationToken;
        IProject? initProject = null;

        Log.Information(">> Starting {library}", "Autoshortcut");

        var factory = ServiceProvider.GetRequiredService<IShortcutEngineFactory>();
        var engine = factory.CreateEngine();

        Log.Information("Project Mode '{mode}'", Variables.CurrentProjectOpenMode());
        Log.Information("Get files from '{path}'", FilesDirectoryPath.Value);

        const string projectRootDirectory = "./[AutoShortcuts]Projects/Compilation-1.ash";

        if (Variables.CurrentProjectOpenMode() == ProjectModes.Restore)
        {
            var loader = ServiceProvider.GetRequiredService<IRuntimeProjectLoader>();
            await loader.LoadAsync(projectRootDirectory);
            initProject = await OnRestoreProjectAsync(loader);
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
            initProject = await OnCreateProjectAsync(engine, files, projectRootDirectory);
        }

        if (initProject is null)
            throw new InvalidEnvironmentVariableException(
                $"Invalid input variable '{EnvironmentVariableNames.ProjectOpenMode}' is invalid");

        if (initProject is not null)
        {
            var compilation = await engine.StartRenderAsync(initProject, cancellationToken);
            Log.Information("Finally video: " + compilation.Path);
        }
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