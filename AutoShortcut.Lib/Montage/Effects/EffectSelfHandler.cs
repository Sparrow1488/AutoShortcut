using AutoShortcut.Lib.Contracts.Core;
using AutoShortcut.Lib.Contracts.Media;
using AutoShortcut.Lib.Contracts.Montage;
using AutoShortcut.Lib.Contracts.Services;

namespace AutoShortcut.Lib.Montage.Effects;

public abstract class EffectSelfHandler
{
    public EffectSelfHandler(IServiceProvider services)
    {
        ScriptEngine = (IFFmpegEngine) services.GetService(typeof(IFFmpegEngine))!;
        NameService = (INameService) services.GetService(typeof(INameService))!;
    }
    
    public INameService NameService { get; }
    public IFFmpegEngine ScriptEngine { get; }

    public abstract Task<IMediaFile> HandleAsync(IRenderMedia media, CancellationToken ctk = default);
}