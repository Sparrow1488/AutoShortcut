using AutoShortcut.Lib.Contracts;
using AutoShortcut.Lib.Contracts.Enums;

namespace AutoShortcut.Lib.Analyse.Entries;

public class VideoStreamAnalyse : StreamAnalyse, IVideoAnalyse
{
    public int Width { get; set; }
    public int Height { get; set; }
    public string DisplayAspectRatio { get; set; } = string.Empty;
    public override StreamAnalyseKind Kind => StreamAnalyseKind.Video;
}