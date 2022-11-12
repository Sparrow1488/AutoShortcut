using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Rules;

namespace Sparrow.Video.Abstractions.Projects.Options;

public interface IProjectOptions
{
    string ProjectName { get; }
    bool IsSerialize { get; }
    IFilesStructure Structure { get; }
    IFileRulesContainer RulesContainer { get; }
    IProjectRoot Root { get; }
    IEnumerable<string> ProjectFilesPaths { get; }

    IProjectOptions WithRules(Action<IFileRulesContainer> projectRules);
    IProjectOptions StructureBy(IFilesStructure structure);
    IProjectOptions Named(string name);
    IProjectOptions Serialize(bool value);
    IProjectOptions SetRootDirectory(string path);
}