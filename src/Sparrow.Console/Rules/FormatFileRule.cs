using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Shortcuts.Processes.Settings;
using Sparrow.Video.Shortcuts.Rules;

namespace Sparrow.Console.Rules
{
    public class FormatFileRule : FileRuleBase
    {
        public FormatFileRule(
            Resolution resolution,
            FrameFrequency fps,
            Func<IProjectFile, bool> condition) : base(condition)
        {
            Resolution = resolution;
            Fps = fps;
        }

        public Resolution Resolution { get; }
        public FrameFrequency Fps { get; }
        public IVideoFormatSettings FormatSettings => new VideoFormatSettings()
        {
            FileFormat = ".mp4",
            DisplayResolution = Resolution,
            FrameFrequency = Fps
        };
        public override RuleName RuleName => RuleName.New("Format");
    }
}
