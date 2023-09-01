using AutoShortcut.Lib.Contracts.Media;

namespace AutoShortcut.Lib.Contracts.Services;

public interface ITrackCompiler
{
    Task<IMediaFile> CompileAsync(ITrack track, CancellationToken ctk = default);
}