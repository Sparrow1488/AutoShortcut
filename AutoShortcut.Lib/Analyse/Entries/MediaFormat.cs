using AutoShortcut.Lib.Contracts;

namespace AutoShortcut.Lib.Analyse.Entries;

public class MediaFormat : IMediaFormat
{
    public double Duration { get; set; }
    public string? FormatName { get; set; }
}