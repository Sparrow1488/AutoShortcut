using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Shortcuts.Rules;

namespace Sparrow.Console.Rules
{
    public class LoopFileRule : FileRuleBase
    {
        public LoopFileRule(Func<IProjectFile, bool> condition) : base(condition)
        {
        }

        public int LoopCount { get; set; }
        public override RuleName RuleName { get; } = RuleName.New("Loop");
    }
}
