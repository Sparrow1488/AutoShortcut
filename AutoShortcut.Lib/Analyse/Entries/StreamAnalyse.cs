using AutoShortcut.Lib.Contracts;
using AutoShortcut.Lib.Contracts.Enums;

namespace AutoShortcut.Lib.Analyse.Entries;

public abstract class StreamAnalyse : IStreamAnalyse
{
    public int Index { get; set; }
    public string? CodecName { get; set; }
    public double Duration { get; set; }
    public int BitRate { get; set; }
    public abstract StreamAnalyseKind Kind { get; }
}