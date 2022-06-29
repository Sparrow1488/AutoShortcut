using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Projects;

namespace Sparrow.Video.Shortcuts.Render
{
    public interface IRenderUtility
    {
        Task<IFile> StartRenderAsync(
            IProject project, ISaveSettings saveSettings, CancellationToken cancellationToken = default);
    }
}
