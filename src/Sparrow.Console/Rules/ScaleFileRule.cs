using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Shortcuts.Processes.Settings;
using Sparrow.Video.Shortcuts.Rules;

namespace Sparrow.Console.Rules
{
    public class ScaleFileRule : FileRuleBase
    {
        public IScaleSettings Scale => ScaleSettings.Create(Resolution.QHD);
        public override Func<IProjectFile, bool> Condition => file => true;

        public override RuleName RuleName => RuleName.New("Scale");
    }
}
