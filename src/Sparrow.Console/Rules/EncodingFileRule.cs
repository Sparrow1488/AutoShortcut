using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Shortcuts.Rules;

namespace Sparrow.Console.Rules;

public class EncodingFileRule : FileRuleBase
{
    public override Func<IProjectFile, bool> Condition => file => true;
    public string EncodingType => Video.Abstractions.Enums.EncodingType.Mpegts;

    public override RuleName RuleName => RuleName.New("Encoding");

    public override bool Equals(object? obj)
    {
        if (obj is EncodingFileRule encodingRule)
        {
            return encodingRule.EncodingType == EncodingType;
        }
        return false;
    }
}
