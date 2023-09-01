namespace AutoShortcut.Lib.Montage.Effects;

public class FadeOutEffect : FadeEffectBase
{
    public FadeOutEffect(int startSecond, int duration, IServiceProvider services) 
        : base(startSecond, duration, "out", services)
    {
    }
}