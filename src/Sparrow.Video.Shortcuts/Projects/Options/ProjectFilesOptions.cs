using Newtonsoft.Json;
using Sparrow.Video.Abstractions.Projects.Options;

namespace Sparrow.Video.Shortcuts.Projects.Options;

[Serializable]
public class ProjectFilesOptions : IProjectFilesOptions
{
    [JsonConstructor]
    private ProjectFilesOptions(IEnumerable<string> filesPaths)
    {
        FilesPaths = filesPaths;
    }

    [JsonProperty]
    public IEnumerable<string> FilesPaths { get; internal set; }

    public static ProjectFilesOptions Create(IEnumerable<string> filesPaths)
    {
        return new(filesPaths);
    }
}
