using Sparrow.Video.Abstractions.Rules;

namespace Sparrow.Video.Abstractions.Primitives;

public interface IProjectFile
{
    IFile File { get; }
    IFileAnalyse Analyse { get; }
    IFileRulesContainer RulesContainer { get; }
    ICollection<IReference> References { get; }
}
