using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Sparrow.Video.Abstractions.Projects.Options;

namespace Sparrow.Video.Shortcuts.Projects.Options;

[Serializable]
public class ProjectPaths : IProjectPaths
{
    [JsonProperty]
    public string RootPath { get; internal set; }
    [JsonProperty]
    public string FilesDirectoryPath { get; internal set; }

    public string Get(string name)
    {
        throw new NotImplementedException();
    }

    public string GetRequired(string name)
    {
        throw new NotImplementedException();
    }

    public string GetFilesDirectory() => FilesDirectoryPath;
    public string GetRoot() => RootPath;
}

public struct FileStoreInfo
{
    public string Path { get; set; }
}
