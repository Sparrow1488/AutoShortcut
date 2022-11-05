using Sparrow.Video.Abstractions.Rules;

namespace Sparrow.Video.Abstractions.Primitives;

public interface IProjectFile
{
    IFile File { get; }
    IFileAnalyse Analyse { get; }
    ICollection<IFileRule> RulesCollection { get; }
    ICollection<IReference> References { get; }
}
