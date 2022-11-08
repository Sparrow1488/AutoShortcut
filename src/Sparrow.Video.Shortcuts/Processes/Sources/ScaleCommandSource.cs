using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Processes.Abstractions;
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
        var h = Param.Height;
        var w = Param.Width;
        return $"-i \"{Param.FromFilePath}\" -vf \"[in] scale=iw* min({w}/iw\\,{h}/ih):ih* min({w}/iw\\,{h}/ih)[scaled]; [scaled] pad={w}:{h}:({w}-iw* min({h}/iw\\,{h}/ih))/2:({h}-ih* min({w}/iw\\,{h}/ih))/2[padded]; [padded] setsar=1:1[out]\" -c:v libx264 -c:a copy";
    }
}
