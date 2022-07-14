using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Rules;

namespace Sparrow.Console.Rules
{
    public class SilentFileRule : FileRuleBase
    {
        public override Func<IProjectFile, bool> Condition => file => !file.Analyse.StreamAnalyses.WithAudio();

        public override RuleName RuleName => RuleName.Silent;
    }
}
