namespace AutoShortcut.Lib.Contracts.Services.Primitives;

public class ProcessRunInfo
{
    public bool ShowConsole { get; set; }
    public string? Arguments { get; set; }
    public required string FileName { get; init; }
}