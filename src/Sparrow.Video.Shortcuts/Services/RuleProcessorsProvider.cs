using Microsoft.Extensions.DependencyInjection;
using Sparrow.Video.Abstractions.Processors;
using Sparrow.Video.Abstractions.Rules;
using Sparrow.Video.Abstractions.Services;
using System.Reflection;

namespace Sparrow.Video.Shortcuts.Services
{
    public class RuleProcessorsProvider : IRuleProcessorsProvider
    {
        public RuleProcessorsProvider(
            IServiceProvider services)
        {
            Services = services;
        }

        public IServiceProvider Services { get; }

        public IRuleProcessor<TRule> GetRuleProcessor<TRule>() where TRule : IFileRule
        {
            return (IRuleProcessor<TRule>)GetRuleProcessor(typeof(TRule));
        }

        public object GetRuleProcessor(Type ruleType)
        {
            var types = Assembly.GetAssembly(ruleType).GetTypes();
            var assemblyProcessorTypes = types.Where(x => x.IsAssignableTo(typeof(IRuleProcessor)));
            var ruleProcessors = new List<IRuleProcessor>();
            foreach (var type in assemblyProcessorTypes)
            {
                var created = (IRuleProcessor)ActivatorUtilities.CreateInstance(Services, type);
                ruleProcessors.Add(created);
            }
            var found = ruleProcessors.Where(p => p.GetRuleType() == ruleType).FirstOrDefault();
            return found;
        }
    }
}
