using Sparrow.Console.Rules;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Shortcuts.Processors;

namespace Sparrow.Console.Processors
{
    public class GroupRuleProcessor : RuleProcessorBase<GroupFileRule>
    {
        public override Task ProcessAsync(IProjectFile file, GroupFileRule rule)
        {
            throw new NotImplementedException();
        }
    }
}
