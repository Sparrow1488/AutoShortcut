using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Services;

namespace Sparrow.Video.Shortcuts.Services;

public class DefaultSaveService : SaveService, IDefaultSaveService
{
    public DefaultSaveService(
        IPathsProvider pathsProvider) 
    : base(pathsProvider)
    {
    }

    public async Task SaveAsync(
        string log, ISaveSettings saveSettings, CancellationToken cancellationToken = default)
    {
        await SaveTextAsync(log, saveSettings, cancellationToken);
    }
}
