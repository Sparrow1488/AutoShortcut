using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Processes.Sources.Parameters;

namespace Sparrow.Video.Shortcuts.Processes.Sources;

public class EncodingCommandSource : FFmpegCommandSource<EncodingCommandParameters>
{
    public EncodingCommandSource(EncodingCommandParameters param) : base(param) { }

    public override string ProjectConfigSection => ProjectConfigSections.EncodedFiles;

    public override string GetCommand()
    {
        return $"-i \"{Param.FromFilePath}\" -acodec copy -vcodec copy -vbsf h264_mp4toannexb -crf 17 -f {Param.EncodingType}";
    }
}
