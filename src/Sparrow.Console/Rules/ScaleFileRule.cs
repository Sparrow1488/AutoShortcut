using Newtonsoft.Json;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Rules;
using Sparrow.Video.Shortcuts.Rules;

namespace Sparrow.Console.Rules;

[Serializable]
public class ScaleFileRule : PermanentFileRule
{
    public ScaleFileRule(Resolution resolution)
    {
        Resolution = resolution;
    }

    public Resolution Resolution { get; set; }
    public override Func<IProjectFile, bool> Condition => file => true;

    public override RuleName RuleName => RuleName.Scale;
    public override IFileRule Clone()
    {
        return new ScaleFileRule(Resolution);
    }
}
