using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes.Settings;

namespace Sparrow.Video.Abstractions.Processes
{
    public interface IScaleProcess
    {
        Task<IFile> ScaleVideoAsync(
            IFile file, 
            IScaleSettings scaleSettings,
            ISaveSettings saveSettings,
            CancellationToken cancellationToken = default);
    }
}
