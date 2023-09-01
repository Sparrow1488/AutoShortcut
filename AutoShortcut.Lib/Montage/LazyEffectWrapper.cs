using AutoShortcut.Lib.Contracts.Media;
using AutoShortcut.Lib.Contracts.Montage;
using AutoShortcut.Lib.Exceptions;

namespace AutoShortcut.Lib.Montage;

public class LazyEffectWrapper : ILazyEffect
{
    private readonly Func<IMediaFile?, IEffect>? _factory;

    public LazyEffectWrapper(IEffect effect)
    {
        Effect = effect;
    }

    public LazyEffectWrapper(Func<IMediaFile?, IEffect> factory)
    {
        _factory = factory;
    }
    
    public IEffect? Effect { get; private set; }
    public bool EffectLoaded => Effect is not null;

    public IEffect LoadEffect(IMediaFile? previousHandled)
    {
        if (_factory is null && Effect is null) throw new HandleException("No effect factory method to load lazy effect");

        if (_factory is not null && Effect is null)
            Effect = _factory.Invoke(previousHandled);

        return Effect ?? throw new HandleException("Failed to load effect");
    }
}