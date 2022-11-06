using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Rules;

namespace Sparrow.Video.Abstractions.Processors
{
    public interface IRuleProcessor<in TRule> : IRuleProcessor
        where TRule : IFileRule
    {
        Task<IFile> ProcessAsync(IProjectFile file, TRule rule, CancellationToken cancellationToken = default);
    }

    public interface IRuleProcessor : IProcessor
    {
        Type GetRuleType();
        Task<IFile> ProcessAsync(IProjectFile file, IFileRule rule, CancellationToken cancellationToken = default);
    }
}
