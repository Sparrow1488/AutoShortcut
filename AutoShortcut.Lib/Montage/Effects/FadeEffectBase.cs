using AutoShortcut.Lib.Contracts.Core;
using AutoShortcut.Lib.Contracts.Media;
using AutoShortcut.Lib.Core;

namespace AutoShortcut.Lib.Montage.Effects;

public abstract class FadeEffectBase : EffectSelfHandler<FadeOutEffect>, IVideoEffect
{
    private readonly string _suffix;

    /// <summary>
    /// Создает экземпляр класа
    /// </summary>
    /// <param name="startSecond"></param>
    /// <param name="duration"></param>
    /// <param name="suffix">in or out</param>
    /// <param name="services"></param>
    protected FadeEffectBase(int startSecond, int duration, string suffix, IServiceProvider services) : base(services)
    {
        _suffix = suffix;
        StartSecond = startSecond;
        Duration = duration;
    }

    public int StartSecond { get; }
    public int Duration { get; }
    
    protected override Script NewScript(IMediaFile changing)
    {
        return new FadeScript(changing.Path, StartSecond, Duration, _suffix);
    }
}