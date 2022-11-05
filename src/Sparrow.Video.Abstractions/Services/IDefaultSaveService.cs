using Sparrow.Video.Abstractions.Processes.Settings;

namespace Sparrow.Video.Abstractions.Services;

public interface IDefaultSaveService
{
    Task SaveAsync(
        string text, ISaveSettings saveSettings, CancellationToken cancellationToken = default);
}
