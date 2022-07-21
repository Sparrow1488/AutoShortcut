using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processors;
using Sparrow.Video.Abstractions.Rules;
using Sparrow.Video.Shortcuts.Exceptions;

namespace Sparrow.Video.Shortcuts.Processors
{
    public abstract class RuleProcessorBase<TRule> : IRuleProcessor<TRule>
        where TRule : IFileRule
    {
        public abstract Task ProcessAsync(IProjectFile file, TRule rule);

        public Type GetRuleType()
        {
            return typeof(TRule);
        }

        public async Task ProcessAsync(IProjectFile file, IFileRule rule)
        {
            if (rule.GetType().IsAssignableTo(GetRuleType()) || rule.GetType() == GetRuleType())
            {
                await ProcessAsync(file, (TRule)rule);
            }
            else
            {
                throw new FailedProcessRuleException("Processor can't handle current rule type " + rule.RuleName.Value);
            }
        }
    }
}
