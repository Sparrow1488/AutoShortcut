using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Rules
{
    public class VideoFormatFileRule : FileRuleBase
    {
        public VideoFormatFileRule(
            IVideoFormatSettings formatSettings, 
            Func<IProjectFile, bool> condition) : base(condition)
        {
            FormatSettings = formatSettings;
        }

        public override RuleName RuleName => RuleName.Formating;
        public IVideoFormatSettings FormatSettings { get; }
    }
}
