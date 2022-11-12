namespace Sparrow.Video.Abstractions.Processes.Sources;

public interface IFFmpegCommandSource
{
    string ProjectConfigSection { get; }
    string SaveFileName { get; }
    string GetCommand();
}
