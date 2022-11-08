using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Shortcuts.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Processes.Sources.Parameters;

public class ScaleCommandParameters : CommandParameters
{
    public int Height { get; set; }
    public int Width { get; set; }
}
