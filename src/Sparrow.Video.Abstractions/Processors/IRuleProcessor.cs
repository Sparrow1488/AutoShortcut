using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Rules;

namespace Sparrow.Video.Abstractions.Processors
{
    public interface IRuleProcessor<in TRule> : IRuleProcessor
        where TRule : IFileRule
    {
        Task ProcessAsync(IProjectFile file, TRule rule);
    }

    public interface IRuleProcessor : IProcessor
    {
        Type GetRuleType();
        Task ProcessAsync(IProjectFile file, IFileRule rule);
    }
}
