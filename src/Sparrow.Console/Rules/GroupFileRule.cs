using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Shortcuts.Rules;

namespace Sparrow.Console.Rules
{
    public class GroupFileRule : FileRuleBase
    {
        public GroupFileRule(
            IEnumerable<string> groups, 
            Func<IProjectFile, bool> condition) : base(condition)
        {
            Groups = groups;
        }

        public IEnumerable<string> Groups { get; set; }
        public override RuleName RuleName => RuleName.New("Group");
    }
}
