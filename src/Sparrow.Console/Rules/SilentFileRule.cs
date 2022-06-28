using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Shortcuts.Rules;

namespace Sparrow.Console.Rules
{
    public class SilentFileRule : FileRuleBase
    {
        public SilentFileRule(Func<IProjectFile, bool> condition) : base(condition)
        {
        }

        public override RuleName RuleName => RuleName.Silent;
    }
}
