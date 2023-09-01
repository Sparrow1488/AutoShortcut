using AutoShortcut.Lib.Contracts.Montage;

namespace AutoShortcut.Lib.Montage.Changes;

public sealed class EffectChangeMeta
{
    public EffectChangeMeta(IEffect applied)
    {
        Applied = applied;
    }
    
    public IEffect Applied { get; }
}