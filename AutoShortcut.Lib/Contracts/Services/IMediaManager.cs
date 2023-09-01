using AutoShortcut.Lib.Contracts.Media;

namespace AutoShortcut.Lib.Contracts.Services;

public interface IMediaManager
{
    Task<IMediaFile> LoadAsync(string path, CancellationToken ctk = default);
    Task<IMediaFile> LoadAnalysedAsync(string path, CancellationToken ctk = default);
}