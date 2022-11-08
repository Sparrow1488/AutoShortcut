using Sparrow.Video.Shortcuts.Processes.Sources.Parameters;

namespace Sparrow.Video.Shortcuts.Processes.Sources;

public abstract class FFmpegCommandSource<TParam> : IFFmpegCommandSource
    where TParam : CommandParameters
{
    public FFmpegCommandSource(TParam param)
    {
        Param = param;
    }

    public abstract string ProjectConfigSection { get; }
    public string SaveFileName => Param.SaveFileName;

    public TParam Param { get; }

    public abstract string GetCommand();
}

public interface IFFmpegCommandSource
{
    string ProjectConfigSection { get; }
    string SaveFileName { get; }
    string GetCommand();
}
