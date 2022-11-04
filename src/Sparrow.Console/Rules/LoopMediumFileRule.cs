using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Shortcuts.Extensions;

namespace Sparrow.Console.Rules;

public class LoopMediumFileRule : LoopFileRuleBase
{
    public override int LoopCount => 2;
    public override Func<IProjectFile, bool> Condition =>
        file => file.Analyse.StreamAnalyses.Video().Duration > 8 &&
                file.Analyse.StreamAnalyses.Video().Duration <= 13;
}
