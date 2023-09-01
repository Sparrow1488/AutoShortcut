using AutoShortcut.Lib.Contracts.Core;
using AutoShortcut.Lib.Contracts.Media;
using AutoShortcut.Lib.Core;

namespace AutoShortcut.Lib.Montage.Effects;

/// <summary>
/// Эффект прозрачности видео. Устанавливаемый параметр от 0 до 1
/// </summary>
public class OpacityEffect : EffectSelfHandler<FadeInEffect>, IVideoEffect
{
    public OpacityEffect(double opacity, IServiceProvider services) : base(services)
    {
        if (opacity is < 0 or > 1)
            throw new Exception();

        Opacity = opacity;
    }

    /// <summary>
    /// Прозрачность видео от 0 до 1
    /// </summary>
    public double Opacity { get; }
    
    protected override Script NewScript(IMediaFile changing)
    {
        return new OpacityScript(Opacity);
    }
}