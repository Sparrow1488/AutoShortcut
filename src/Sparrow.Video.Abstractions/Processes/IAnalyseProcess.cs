using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Abstractions.Processes;

public interface IAnalyseProcess : IExecutionProcess
{
    Task<IFileAnalyse> GetAnalyseAsync(IFile file, CancellationToken cancellationToken = default);
}