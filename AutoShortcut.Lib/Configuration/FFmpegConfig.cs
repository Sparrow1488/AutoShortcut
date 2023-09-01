using AutoShortcut.Lib.Contracts.Services.Primitives;

namespace AutoShortcut.Lib.Configuration;

public sealed class FFmpegConfig
{
    public FFmpegConfig(string ffmpegPath, string ffprobePath)
    {
        FFmpegPath = ffmpegPath;
        FFprobePath = ffprobePath;
    }
    
    public string FFmpegPath { get; }
    public string FFprobePath { get; }
    public ProcessRunInfo FFmpegRunInfo => new() { FileName = FFmpegPath };
    public ProcessRunInfo FFprobeRunInfo => new() { FileName = FFprobePath };
}