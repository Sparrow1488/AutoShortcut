using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Processes.Sources.Parameters;

namespace Sparrow.Video.Shortcuts.Processes.Sources;

public class SilentCommandSource : FFmpegCommandSource<SilentCommandParameters>
{
    public SilentCommandSource(SilentCommandParameters param) : base(param) { }

    public override string ProjectConfigSection => ProjectConfigSections.SilentFiles;

    public override string GetCommand()
    {
        return $"-f lavfi -i anullsrc=channel_layout=stereo:sample_rate=44100 -i \"{Param.FromFilePath}\" -c:v copy -c:a aac -shortest";
    }
}
