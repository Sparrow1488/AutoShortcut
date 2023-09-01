using AutoShortcut.Lib.Contracts.Core;
using AutoShortcut.Lib.Contracts.Media;
using AutoShortcut.Lib.Core;
using AutoShortcut.Lib.Media;

namespace AutoShortcut.Lib.Montage.Effects;

public class ScaleEffect : EffectSelfHandler<ScaleEffect>, IVideoEffect
{
    public ScaleEffect(Resolution resolution, IServiceProvider services) : base(services)
    {
        Resolution = resolution;
    }
    
    public Resolution Resolution { get; }

    protected override Script NewScript(IMediaFile changing)
    {
        return new ScaleVideoScript(changing.Path, Resolution.Width, Resolution.Height);
    }
}