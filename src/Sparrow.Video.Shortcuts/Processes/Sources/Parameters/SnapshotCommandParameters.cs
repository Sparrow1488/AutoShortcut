namespace Sparrow.Video.Shortcuts.Processes.Sources.Parameters;

public class SnapshotCommandParameters : CommandParameters
{
    public SnapshotCommandParameters(string saveFileName, string fromFilePath) 
    : base(saveFileName, fromFilePath)
    {
    }

    public TimeSpan Time { get; set; }
}