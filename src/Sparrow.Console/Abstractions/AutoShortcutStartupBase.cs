using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Render;
using Sparrow.Video.Abstractions.Runtime;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Exceptions;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Services.Options;

namespace Sparrow.Console.Abstractions;

internal abstract class AutoShortcutStartupBase : Startup
{
    public CancellationToken CancellationToken { get; private set; }

    public abstract Task<IProject> OnRestoreProjectAsync(IRuntimeProjectLoader loader);
    public abstract Task<IProject> OnCreateProjectAsync(
        IRuntimeProjectLoader loader, IEnumerable<IFile> files, string projectPath);

    public override async Task OnStart(CancellationToken cancellationToken = default)
    {
        CancellationToken = cancellationToken;
        IProject? initProject = null;

        Log.Information(">> Starting {library}", "Autoshortcut");

        var renderUtility = ServiceProvider.GetRequiredService<IRenderUtility>();

        Log.Information("Project Mode '{mode}'", Variables.CurrentProjectOpenMode());
        Log.Information("Get files from '{path}'", FilesDirectoryPath.Value);

        const string projectRootDirectory = "./[AutoShortcuts]Projects/Compilation-1.ash";

        var loader = ServiceProvider.GetRequiredService<IRuntimeProjectLoader>();

        if (Variables.CurrentProjectOpenMode() == ProjectModes.Restore)
        {
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
            initProject = await OnCreateProjectAsync(loader, files, projectRootDirectory);
        }

        if (initProject is null)
            throw new InvalidEnvironmentVariableException(
                $"Invalid input variable '{EnvironmentVariableNames.ProjectOpenMode}' is invalid");

        if (initProject is not null)
        {
            var compilation = await renderUtility.StartRenderAsync(initProject, cancellationToken);
            Log.Information("Finally video: " + compilation.Path);
        }
    }

    protected static UploadFilesOptions GetUploadOptions()
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
