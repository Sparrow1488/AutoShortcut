using Newtonsoft.Json;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects.Options;
using Sparrow.Video.Abstractions.Rules;
using Sparrow.Video.Shortcuts.Primitives.Structures;
using Sparrow.Video.Shortcuts.Rules;

namespace Sparrow.Video.Shortcuts.Projects.Options;

public class ProjectOptions : IProjectOptions
{
    public ProjectOptions()
    {
        Structure = DefaultStructure;
    }

    [JsonConstructor]
    internal ProjectOptions(
        IFilesStructure structure, IFileRulesContainer rulesContainer, string projectName)
    {
        Structure = structure;
        RulesContainer = rulesContainer;
        ProjectName = projectName;
    }

    [JsonProperty]
    public IFilesStructure Structure { get; private set; }
    [JsonProperty]
    public string ProjectName { get; private set; } = $"Project_{DateTime.Now.Millisecond}";
    [JsonProperty]
    public IFileRulesContainer RulesContainer { get; } = new FileRulesContainer();
    [JsonIgnore]
    public IFilesStructure DefaultStructure { get; } = new NameStructure();

    public IProjectOptions Named(string name)
    {
        ProjectName = name.Trim();
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
}
