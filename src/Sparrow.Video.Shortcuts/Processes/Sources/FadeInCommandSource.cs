using System.Globalization;
using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Processes.Sources.Parameters;

namespace Sparrow.Video.Shortcuts.Processes.Sources;

public class FadeInCommandSource : FFmpegCommandSource<FadeInCommandParameter>
{
    public FadeInCommandSource(FadeInCommandParameter param) : base(param) { }

    public override string ProjectConfigSection => ProjectConfigSections.FadeIn;
    
    public override string GetCommand()
    {
        return $"-i \"{Param.FromFilePath}\" -vf fade=in:st=0:d={Param.Seconds.ToString(CultureInfo.InvariantCulture).Replace(",", ".")}";
    }
}