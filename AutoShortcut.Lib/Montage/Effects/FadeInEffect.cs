namespace AutoShortcut.Lib.Montage.Effects;

public class FadeInEffect : FadeEffectBase
{
    public FadeInEffect(int startSecond, int duration, IServiceProvider services) 
        : base(startSecond, duration, "in", services)
    {
    }
}