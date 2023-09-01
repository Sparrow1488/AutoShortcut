using AutoShortcut.Lib.Contracts.Core;
using AutoShortcut.Lib.Contracts.Media;
using AutoShortcut.Lib.Core;

namespace AutoShortcut.Lib.Montage.Effects;

public class FpsEffect : EffectSelfHandler<FpsEffect>, IVideoEffect
{
    private readonly int _fps;

    public FpsEffect(int fps, IServiceProvider services) : base(services)
    {
        _fps = fps;
    }

    protected override Script NewScript(IMediaFile changing)
    {
        return new FpsVideoScript(changing.Path, _fps);
    }
}