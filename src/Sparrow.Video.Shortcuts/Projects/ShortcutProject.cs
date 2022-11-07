using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Projects.Options;

namespace Sparrow.Video.Shortcuts.Projects;

public class ShortcutProject : IProject
{
    private ShortcutProject() { }

    private ShortcutProject(IProjectOptions options) 
        => Options = options;

    private ShortcutProject(
        IProjectOptions options,
        IEnumerable<IProjectFile> files)
    {
        Options = options;
        Files = files;
    }

    public IEnumerable<IProjectFile> Files { get; internal set; }
    public IProjectOptions Options { get; internal set; }

    public IProject ConfigureOptions(Action<IProjectOptions> configureOptions)
    {
        configureOptions.Invoke(Options);
        return this;
    }

    internal static ShortcutProject Create() => new();

    public static ShortcutProject Create(IProjectOptions options) 
        => new(options);

    public static ShortcutProject Create(
        IProjectOptions options, IEnumerable<IProjectFile> files) 
            => new(options, files);
}
