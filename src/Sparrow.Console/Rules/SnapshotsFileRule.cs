using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Shortcuts.Rules;

namespace Sparrow.Console.Rules;

public class SnapshotsFileRule : FileRuleBase
{
    public int Count => 5;

    public override RuleName RuleName => RuleName.Snapshot;
    public override Func<IProjectFile, bool> Condition => file => true;
}
