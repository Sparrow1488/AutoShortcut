namespace Sparrow.Video.Shortcuts.Processes.Sources.Parameters;

public class FadeInCommandParameter : CommandParameters
{
    public FadeInCommandParameter(
        string saveFileName, string fromFilePath) 
    : base(saveFileName, fromFilePath)
    {
    }

    public double Seconds { get; set; }
}