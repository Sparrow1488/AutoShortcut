using Sparrow.Video.Abstractions.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Processes.Settings;

public class SaveSettings : ISaveSettings
{
    public string SaveFullPath { get; set; }

    public static ISaveSettings Create(string fullPath)
    {
        return new SaveSettings() { SaveFullPath = fullPath };
    }
}
