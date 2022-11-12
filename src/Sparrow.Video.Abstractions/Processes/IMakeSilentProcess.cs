using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes.Settings;

namespace Sparrow.Video.Abstractions.Processes
{
    public interface IMakeSilentProcess
    {
        Task<IFile> MakeSilentAsync(IFile file, ISaveSettings saveSettings);
    }
}
