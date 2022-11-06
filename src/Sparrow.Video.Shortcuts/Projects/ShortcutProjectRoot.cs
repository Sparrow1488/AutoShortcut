using Newtonsoft.Json;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Projects.Options;
using Sparrow.Video.Shortcuts.Projects.Options;

namespace Sparrow.Video.Shortcuts.Projects;

[Serializable]
public class ShortcutProjectRoot : IProjectRoot
{
    private ShortcutProjectRoot() { }

    [JsonConstructor]
    private ShortcutProjectRoot(IProjectPaths projectPaths)
    {
        ProjectPaths = projectPaths;
    }

    public static ShortcutProjectRoot Default => new();

    [JsonProperty]
    public IProjectPaths ProjectPaths { get; private set; } = new ProjectPaths();

    public IProjectRoot WithPaths(IProjectPaths paths)
    {
        ProjectPaths = paths;
        return this;
    }
}
