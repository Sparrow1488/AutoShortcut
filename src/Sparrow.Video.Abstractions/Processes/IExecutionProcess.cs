namespace Sparrow.Video.Abstractions.Processes;

public interface IExecutionProcess
{
    Task StartAsync(CancellationToken cancellationToken = default);
}