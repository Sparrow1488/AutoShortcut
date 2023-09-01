using AutoShortcut.Lib.Contracts;
using AutoShortcut.Lib.Contracts.Media;
using AutoShortcut.Lib.Contracts.Montage;
using AutoShortcut.Lib.Exceptions;
using AutoShortcut.Lib.Montage.Changes;

namespace AutoShortcut.Lib.Montage;

public class RenderMedia : IRenderMedia
{
    private readonly List<ILazyEffect> _lazyEffects = new();
    private readonly List<EffectChange> _changes = new();
    
    public RenderMedia(IMediaFile main)
    {
        Main = main;
    }
    
    public IMediaFile Main { get; }
    public IEnumerable<ILazyEffect> Effects => _lazyEffects.AsReadOnly();
    public Change<EffectChangeMeta>? LastChange => _changes.MaxBy(x => x.ChangedAt);
    public IEnumerable<Change<EffectChangeMeta>> Changes => _changes.AsReadOnly();

    public IRenderMedia AddEffect(IEffect effect)
    {
        _lazyEffects.Add(new LazyEffectWrapper(effect));
        return this;
    }

    public IRenderMedia AddEffect(Func<IMediaFile?, IEffect> factory)
    {
        _lazyEffects.Add(new LazyEffectWrapper(factory));
        return this;
    }

    public IRenderMedia AddEffects(IEnumerable<IEffect> effects)
    {
        _lazyEffects.AddRange(effects.Select(x => new LazyEffectWrapper(x)));
        return this;
    }

    public void MarkAsApplied(IEffect applied, IMediaFile reference)
    {
        var loadedEffects = _lazyEffects.TakeWhile(x => x.EffectLoaded).Select(x => x.Effect!).ToList();
        if (!loadedEffects.Contains(applied))
            throw new EffectApplyException($"Passed effect not contains in {nameof(Effects)}. Try to add effect before mark it as applied");
        
        _changes.Add(new EffectChange
        {
            Source = reference,
            Meta = new EffectChangeMeta(applied)
        });
    }
}