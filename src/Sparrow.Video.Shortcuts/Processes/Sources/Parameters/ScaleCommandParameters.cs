namespace Sparrow.Video.Shortcuts.Processes.Sources.Parameters;

public class ScaleCommandParameters : CommandParameters
{
    public ScaleCommandParameters(string saveFileName, string fromFilePath) 
    : base(saveFileName, fromFilePath)
    {
    }

    public int Height { get; set; }
    public int Width { get; set; }
}
