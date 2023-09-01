using Sparrow.Video.Abstractions.Processors;
using Sparrow.Video.Abstractions.Rules;

namespace Sparrow.Video.Abstractions.Services;

public interface IRuleProcessorsProvider
{
    IRuleProcessor<TRule>? GetRuleProcessor<TRule>()
        where TRule : IFileRule;
    object? GetRuleProcessor(Type ruleType);
}