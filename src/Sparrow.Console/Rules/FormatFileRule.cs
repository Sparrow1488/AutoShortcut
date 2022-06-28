using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Shortcuts.Processes.Settings;
using Sparrow.Video.Shortcuts.Rules;

namespace Sparrow.Console.Rules
{
    public class FormatFileRule : FileRuleBase
    {
        public FormatFileRule(Func<IProjectFile, bool> condition) : base(condition)
        {
        }

        public IVideoFormatSettings FormatSettings { get; } = new VideoFormatSettings()
        {
            FileFormat = ".mp4",
            DisplayResolution = Resolution.Small
        };
        public override RuleName RuleName => RuleName.New("Format");
    }
}
