using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes.Sources;

namespace Sparrow.Video.Abstractions.Processes;

public interface IFFmpegProcess
{
    Task<IFile> StartAsync(
        IFFmpegCommandSource source, 
        CancellationToken cancellation = default);
}