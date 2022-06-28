using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processors;
using Sparrow.Video.Abstractions.Rules;

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
            if (rule.GetType() != GetRuleType())
                throw new Exception("Processor can't handle current rule type");
            await ProcessAsync(file, (TRule)rule);
        }
    }
}
