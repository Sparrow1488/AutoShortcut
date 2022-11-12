﻿using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Projects.Options;

namespace Sparrow.Video.Abstractions.Runtime;

public interface IRuntimeProjectLoader
{
    IEnumerable<IProjectFile> ProjectFiles { get; }

    IProject CreateProject();
    void LoadEmpty();
    Task LoadAsync(string projectPath);
    Task AddFileAsync(IFile file, CancellationToken cancellationToken = default);
    Task AddFilesAsync(IEnumerable<IFile> files, CancellationToken cancellationToken = default);
    void ConfigureProjectOptions(Action<IProjectOptions> options);
}
