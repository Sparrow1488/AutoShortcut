using AutoShortcut.Lib.Contracts.Media;

namespace AutoShortcut.Lib.Contracts.Montage;

public interface ILazyEffect
{
    IEffect? Effect { get; }
    bool EffectLoaded { get; }
    IEffect LoadEffect(IMediaFile? previousHandled);
}