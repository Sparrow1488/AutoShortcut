using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Shortcuts.Processes.Settings;
using Sparrow.Video.Shortcuts.Rules;

namespace Sparrow.Console.Rules
{
    public class ScaleFileRule : FileRuleBase
    {
        public ScaleFileRule(Resolution resolution, Func<IProjectFile, bool> condition) : base(condition)
        {
            Scale = ScaleSettings.Create(resolution);
        }

        public IScaleSettings Scale { get; }
        public override RuleName RuleName => RuleName.New("Scale");
    }
}
