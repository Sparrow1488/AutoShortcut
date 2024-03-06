using AutoShortcut.Lib.Contracts.Montage;
using AutoShortcut.Lib.Media;
using AutoShortcut.Lib.Montage.Effects;

namespace AutoShortcut.Lib.Extensions;

public static class RenderMediaExtensions
{
    public static IRenderMedia Scale(this IRenderMedia media, Resolution resolution, IServiceProvider di)
    {
        return media.AddEffect(new ScaleEffect(resolution, di));
    }
    
    public static IRenderMedia Loop(this IRenderMedia media, int loopCount, IServiceProvider di)
    {
        return media.AddEffect(new LoopVideo(loopCount, di));
    }
    
    public static IRenderMedia FadeIn(this IRenderMedia media, int seconds, IServiceProvider di)
    {
        return media.AddEffect(new FadeInEffect(0, seconds, di));
    }
    
    public static IRenderMedia FadeOut(this IRenderMedia media, int seconds, IServiceProvider di)
    {
        return media.AddEffect(previous 
            => new FadeOutEffect((int) previous!.MediaFormat!.Duration - seconds, seconds, di)
        );
    }
}