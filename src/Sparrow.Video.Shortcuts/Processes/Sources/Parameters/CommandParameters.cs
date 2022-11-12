namespace Sparrow.Video.Shortcuts.Processes.Sources.Parameters;

public abstract class CommandParameters
{
    public CommandParameters(string saveFileName, string fromFilePath)
    {
        SaveFileName = saveFileName;
        FromFilePath = fromFilePath;
    }

    public string SaveFileName { get; }
    public string FromFilePath { get; }
}