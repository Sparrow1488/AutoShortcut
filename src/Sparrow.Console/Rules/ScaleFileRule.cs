using Newtonsoft.Json;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Rules;
using Sparrow.Video.Shortcuts.Processes.Settings;
using Sparrow.Video.Shortcuts.Rules;

namespace Sparrow.Console.Rules;

[Serializable]
public class ScaleFileRule : FileRuleBase
{
    [JsonConstructor]
    public ScaleFileRule(Resolution resolution)
    {
        Resolution = resolution;
    }

    public Resolution Resolution { get; }
    public override Func<IProjectFile, bool> Condition => file => true;

    public override RuleName RuleName => RuleName.New("Scale");
    public override IFileRule Clone()
    {
        return new ScaleFileRule(Resolution);
    }

    public override bool Equals(object? obj)
    {
        if(obj is ScaleFileRule scaleRule)
        {
            return scaleRule.Resolution.Width == Resolution.Width && 
                   scaleRule.Resolution.Height == Resolution.Height;
        }
        return false;
    }
}
