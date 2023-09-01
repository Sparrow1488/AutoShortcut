using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes.Settings;

namespace Sparrow.Video.Abstractions.Processes;

public interface IConcatinateProcess
{
    Task<IFile> ConcatinateFilesAsync(
        IEnumerable<string> filesPaths, ISaveSettings saveSettings, CancellationToken cancellationToken = default);
}