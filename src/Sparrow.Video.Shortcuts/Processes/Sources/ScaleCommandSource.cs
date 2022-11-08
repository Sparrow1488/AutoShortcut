using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Processes.Sources.Parameters;

namespace Sparrow.Video.Shortcuts.Processes.Sources;

public class ScaleCommandSource : FFmpegCommandSource<ScaleCommandParameters>
{
    public ScaleCommandSource(ScaleCommandParameters param) : base(param)
    {
    }

    public override string ProjectConfigSection => ProjectConfigSections.ScaledFiles;

    public override string GetCommand()
    {
        return $"-i \"{Param.FromFilePath}\" -vf \"[in] scale=iw* min({Param.Width}/iw\\,{Param.Height}/ih):ih* min({Param.Width}/iw\\,{Param.Height}/ih)[scaled]; [scaled] pad={Param.Width}:{Param.Height}:({Param.Width}-iw* min({Param.Width}/iw\\,{Param.Height}/ih))/2:({Param.Height}-ih* min({Param.Width}/iw\\,{Param.Height}/ih))/2[padded]; [padded] setsar=1:1[out]\" -c:v libx264 -c:a copy";
    }
}
