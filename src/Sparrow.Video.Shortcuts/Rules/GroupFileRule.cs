using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Shortcuts.Rules
{
    public class GroupFileRule : FileRuleBase
    {
        public GroupFileRule(string groupName, Func<IProjectFile, bool> condition) : base(condition)
        {
            GroupName = groupName;
        }

        public override RuleName RuleName => RuleName.Group;
        public string GroupName { get; }
    }
}
