using AutoShortcut.Lib.Contracts.Media;
using AutoShortcut.Lib.Contracts.Montage;

namespace AutoShortcut.Lib.Contracts.Services;

public interface IEffectHandler<in TEffect> : ITryHandle
    where TEffect : IEffect
{
    Task<IMediaFile> HandleAsync(TEffect effect, IRenderMedia media, CancellationToken ctk = default);
}