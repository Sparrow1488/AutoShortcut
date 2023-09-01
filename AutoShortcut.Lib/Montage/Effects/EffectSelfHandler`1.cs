using AutoShortcut.Lib.Contracts.Core;
using AutoShortcut.Lib.Contracts.Media;
using AutoShortcut.Lib.Contracts.Montage;
using AutoShortcut.Lib.Contracts.Services;
using AutoShortcut.Lib.Core;
using AutoShortcut.Lib.Exceptions;

namespace AutoShortcut.Lib.Montage.Effects;

public abstract class EffectSelfHandler<TSelf> : EffectSelfHandler, IEffect, IEffectHandler<TSelf>
where TSelf : IEffect
{
    protected EffectSelfHandler(IServiceProvider services) : base(services)
    {
    }
    
    public Task<IMediaFile> HandleAsync(TSelf effect, IRenderMedia media, CancellationToken ctk = default)
    {
        if (!Equals(effect, this))
            throw new HandleException("This handler cannot process another instance of effect. Self handle only.");

        return HandleAsync(media, ctk);
    }
    
    public override async Task<IMediaFile> HandleAsync(IRenderMedia media, CancellationToken ctk = default)
    {
        var actualMedia = (IMediaFile) (media.LastChange?.Source ?? media.Main);
        var settings = new EffectStoreSettings(extension: actualMedia.Extension);
        UpdateStoreSettings(settings);

        var context = new MediaExecutionContext(MediaStoreType.Temporary, NameService.NewTemporaryName(settings.Extension));
        return (await ScriptEngine.ExecuteAsync(await NewScriptAsync(actualMedia, ctk), context, ctk)).Media;
    }

    protected abstract Script NewScript(IMediaFile changing);

    protected virtual Task<Script> NewScriptAsync(IMediaFile changing, CancellationToken ctk = default) 
        => Task.FromResult(NewScript(changing));
    protected virtual void UpdateStoreSettings(EffectStoreSettings settings) { }
    
    public bool CanHandle(object toHandle)
    {
        return toHandle == this;
    }
}