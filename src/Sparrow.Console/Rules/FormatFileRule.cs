using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Shortcuts.Processes.Settings;
using Sparrow.Video.Shortcuts.Rules;

namespace Sparrow.Console.Rules
{
    public class FormatFileRule : FileRuleBase
    {
        public FormatFileRule(Resolution resolution, Func<IProjectFile, bool> condition) : base(condition)
        {
            Resolution = resolution;
            FormatSettings = new VideoFormatSettings()
            {
                FileFormat = ".mp4",
                DisplayResolution = Resolution
            };
        }

        public Resolution Resolution { get; }
        public IVideoFormatSettings FormatSettings { get; } 
        public override RuleName RuleName => RuleName.New("Format");
    }
}
