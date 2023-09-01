using AutoShortcut.Lib.Contracts.Core;
using AutoShortcut.Lib.Contracts.Media;
using AutoShortcut.Lib.Core;
using AutoShortcut.Lib.Media;

namespace AutoShortcut.Lib.Montage.Effects;

public class EncodingVideo : EffectSelfHandler<EncodingVideo>, IVideoEffect
{
    public EncodingVideo(Encoding encoding, IServiceProvider services) : base(services)
    {
        Encoding = encoding;
    }
    
    public Encoding Encoding { get; }

    protected override Script NewScript(IMediaFile changing)
    {
        return new EncodingVideoScript(changing.Path, Encoding.EncodingFormat);
    }

    protected override void UpdateStoreSettings(EffectStoreSettings settings)
    {
        base.UpdateStoreSettings(settings);

        settings.Extension = Encoding.ExtensionName ?? settings.Extension;
    }
}