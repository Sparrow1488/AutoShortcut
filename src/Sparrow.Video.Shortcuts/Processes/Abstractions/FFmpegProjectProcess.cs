﻿using Microsoft.Extensions.DependencyInjection;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Processes.Sources;

namespace Sparrow.Video.Shortcuts.Processes.Abstractions;

public class DefaultFFmpegProjectProcess : FFmpegProjectProcess
{
    public DefaultFFmpegProjectProcess(IServiceProvider services) : base(services)
    {
    }
}

public abstract class FFmpegProjectProcess : FFmpegProcess, IFFmpegProcess
{
    private readonly IProjectSaveSettingsCreator _projectSaveSettings;
    private string _command = string.Empty;
    private IFFmpegCommandSource _source;

    protected FFmpegProjectProcess(IServiceProvider services) : base(services)
    {
        _projectSaveSettings = services.GetRequiredService<IProjectSaveSettingsCreator>();
    }

    public async Task<IFile> StartAsync(
        IFFmpegCommandSource source,
        CancellationToken cancellation = default)
    {
        _source = source;
        _command = "-y " + source.GetCommand() + $" \"{OnConfigureSaveSettings().SaveFullPath}\"";
        return await StartFFmpegAsync(cancellation);
    }

    protected override string OnConfigureFFmpegCommand() => _command;
    protected override ISaveSettings OnConfigureSaveSettings()
        => _projectSaveSettings.Create(_source.ProjectConfigSection, _source.SaveFileName);
}

public interface IFFmpegProcess
{
    Task<IFile> StartAsync(IFFmpegCommandSource source, CancellationToken cancellation = default);
}