using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Shortcuts.Extensions;

namespace Sparrow.Console.Rules;

public class LoopShortFileRule : LoopFileRuleBase
{
    public override int LoopCount => 3;
    public override Func<IProjectFile, bool> Condition => file => file.Analyse.StreamAnalyses.Video().Duration <= 8;
}
