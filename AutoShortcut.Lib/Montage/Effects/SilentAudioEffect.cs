using AutoShortcut.Lib.Contracts.Core;
using AutoShortcut.Lib.Contracts.Media;
using AutoShortcut.Lib.Core;

namespace AutoShortcut.Lib.Montage.Effects;

/// <summary>
/// Заглушает аудио поток у видео. Если аудио отсутствует, то добавляет пустую дорожку (нужно для избежания конфликтор при склейке)
/// </summary>
public class SilentAudioEffect : EffectSelfHandler<SilentAudioEffect>, IVideoEffect
{
    public SilentAudioEffect(IServiceProvider services) : base(services)
    {
    }

    protected override Script NewScript(IMediaFile changing)
    {
        return new SilentAudioScript(changing.Path);
    }
}