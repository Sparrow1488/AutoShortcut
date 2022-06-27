using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Rules;

namespace Sparrow.Video.Abstractions.Processors
{
    public interface IRuleProcessor<TRule> : IProcessor
        where TRule : IFileRule
    {
        Task ProcessAsync(IProjectFile file, TRule rule);
    }
}
