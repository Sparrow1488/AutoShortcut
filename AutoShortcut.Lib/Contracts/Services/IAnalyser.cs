using AutoShortcut.Lib.Contracts.Media;

namespace AutoShortcut.Lib.Contracts.Services;

public interface IAnalyser
{
    Task AnalyseAsync(IMediaFile file, CancellationToken ctk = default);
}