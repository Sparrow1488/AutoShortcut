using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Shortcuts.Rules;

namespace Sparrow.Console.Rules
{
    public class EncodingFileRule : FileRuleBase
    {
        public EncodingFileRule(Func<IProjectFile, bool> condition) : base(condition)
        {
        }

        public string EncodingType { get; set; }
        public override RuleName RuleName { get; } = RuleName.New("Encoding");
    }
}
