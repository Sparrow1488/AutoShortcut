using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Shortcuts.Rules;

namespace Sparrow.Console.Rules;

public class FadeInFileRule : PermanentFileRule
{
    public override RuleName RuleName => RuleName.New("FadeIn");
    public override Func<IProjectFile, bool> Condition { get; } = file => true;

    public double Seconds => 1;
}