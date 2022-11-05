using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Rules;

namespace Sparrow.Video.Abstractions.Projects.Options;

public interface IProjectOptions
{
    string ProjectName { get; }
    IFilesStructure DefaultStructure { get; }
    IFilesStructure Structure { get; }
    IFileRulesContainer RulesContainer { get; }

    IProjectOptions WithRules(Action<IFileRulesContainer> projectRules);
    IProjectOptions StructureBy(IFilesStructure structure);
    IProjectOptions Named(string name);
}
