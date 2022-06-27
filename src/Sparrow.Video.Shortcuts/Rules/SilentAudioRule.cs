using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Rules
{
    public class SilentAudioRule : FileRuleBase
    {
        public SilentAudioRule(Func<IProjectFile, bool> condition) : base(condition)
        {
        }
        public override RuleName RuleName => RuleName.New("SilentAudio");
        public ISaveSettings SaveSettings { get; set; }

        public static SilentAudioRule Default = 
            new SilentAudioRule(file => !file.Analyse.StreamAnalyses
                .Any(x => x.CodecType.ToLower() == "audio"));
    }
}
