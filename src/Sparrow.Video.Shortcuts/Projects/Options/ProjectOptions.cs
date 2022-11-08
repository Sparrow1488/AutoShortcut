using Newtonsoft.Json;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Projects.Options;
using Sparrow.Video.Abstractions.Rules;
using Sparrow.Video.Primitives;
using Sparrow.Video.Shortcuts.Primitives.Structures;
using Sparrow.Video.Shortcuts.Rules;

namespace Sparrow.Video.Shortcuts.Projects.Options;

[Serializable]
public class ProjectOptions : IProjectOptions
{
    private readonly ShortcutProjectRoot _projectRoot = ShortcutProjectRoot.Default;
    private readonly ProjectPaths _projectPaths = new();

    public ProjectOptions()
    {
        Structure = DefaultStructure;
        _projectPaths = new();
        _projectRoot = ShortcutProjectRoot.Default;
        _projectRoot.WithPaths(_projectPaths);
        Root = _projectRoot;
    }

    [JsonConstructor]
    internal ProjectOptions(
        IFilesStructure structure, 
        IFileRulesContainer rulesContainer,
        string projectName,
        IProjectRoot root,
        IEnumerable<string> projectFilesPaths,
        bool isSerialize)
    {
        Structure = structure;
        RulesContainer = rulesContainer;
        ProjectName = projectName;
        Root = root;
        ProjectFilesPaths = projectFilesPaths;
        IsSerialize = isSerialize;
    }

    [JsonProperty]
    public IFilesStructure Structure { get; private set; }
    [JsonProperty]
    public string ProjectName { get; private set; } = $"Project_{DateTime.Now.Millisecond}";
    [JsonProperty]
    public IFileRulesContainer RulesContainer { get; } = new FileRulesContainer();
    [JsonProperty]
    public IProjectRoot Root { get; private set; }
    [JsonProperty]
    public IEnumerable<string> ProjectFilesPaths { get; internal set; }
    [JsonProperty]
    public bool IsSerialize { get; private set; } = true;

    public static IFilesStructure DefaultStructure => new NameStructure();


    public static ProjectOptions Create() => new();

    public IProjectOptions Named(string name)
    {
        ProjectName = name.Trim();
        return this;
    }

    public IProjectOptions SetRootDirectory(string path)
    {
        _projectPaths.RootPath = StringPath.Create(path).Value;
        Directory.CreateDirectory(_projectPaths.RootPath);
        return this;
    }

    public IProjectOptions StructureBy(IFilesStructure structure)
    {
        Structure = structure;
        return this;
    }

    public IProjectOptions WithRules(Action<IFileRulesContainer> projectRules)
    {
        projectRules?.Invoke(RulesContainer);
        return this;
    }

    public IProjectOptions Serialize(bool value)
    {
        IsSerialize = value;
        return this;
    }
}
