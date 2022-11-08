using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Shortcuts.Rules;

namespace Sparrow.Console.Rules;

public class EncodingFileRule : PermanentFileRule
{
    public override Func<IProjectFile, bool> Condition => file => true;
    public string EncodingType => Video.Abstractions.Enums.EncodingType.Mpegts;
    public override RuleName RuleName => RuleName.Encoding;
}
