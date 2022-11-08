using Sparrow.Video.Shortcuts.Processes.Abstractions;

namespace Sparrow.Video.Shortcuts.Processes;

public class DefaultFFmpegProjectProcess : FFmpegProjectProcess
{
    public DefaultFFmpegProjectProcess(IServiceProvider services) 
    : base(services)
    {
    }
}