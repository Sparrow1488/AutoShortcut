using AutoShortcut.Lib.Contracts.Media;
using AutoShortcut.Lib.Montage.Changes;

namespace AutoShortcut.Lib.Contracts.Montage;

public interface IRenderMedia : IChangesHistory<EffectChangeMeta>
{
    IMediaFile Main { get; }
    IEnumerable<ILazyEffect> Effects { get; }
    IRenderMedia AddEffect(IEffect effect);
    IRenderMedia AddEffect(Func<IMediaFile?, IEffect> factory);
    IRenderMedia AddEffects(IEnumerable<IEffect> effects);
    void MarkAsApplied(IEffect applied, IMediaFile reference);
}