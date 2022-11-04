using Newtonsoft.Json;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Rules;
using Sparrow.Video.Shortcuts.Processes.Settings;
using Sparrow.Video.Shortcuts.Rules;

namespace Sparrow.Console.Rules;

public class ScaleFileRule : FileRuleBase
{
    [JsonProperty]
    protected Resolution resolution;

    [JsonConstructor]
    public ScaleFileRule() { }

    public ScaleFileRule(Resolution resolution)
    {
        this.resolution = resolution;
    }

    public IScaleSettings Scale => ScaleSettings.Create(resolution);
    public override Func<IProjectFile, bool> Condition => file => true;

    public override RuleName RuleName => RuleName.New("Scale");
    public override IFileRule Clone()
    {
        return new ScaleFileRule(resolution);
    }
}
