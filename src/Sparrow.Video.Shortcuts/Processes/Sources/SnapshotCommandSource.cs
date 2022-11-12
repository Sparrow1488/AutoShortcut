using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Processes.Sources.Parameters;

namespace Sparrow.Video.Shortcuts.Processes.Sources;

public class SnapshotCommandSource : FFmpegCommandSource<SnapshotCommandParameters>
{
    public SnapshotCommandSource(SnapshotCommandParameters param) : base(param) { }

    public override string ProjectConfigSection => ProjectConfigSections.Snapshots;

    public override string GetCommand()
    {
        return $"-ss {Param.Time.ToString(@"hh\:mm\:ss")} -i \"{Param.FromFilePath}\" -frames:v 1 -q:v 2";
    }
}
