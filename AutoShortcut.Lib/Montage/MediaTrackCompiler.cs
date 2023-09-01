using AutoShortcut.Lib.Configuration;
using AutoShortcut.Lib.Contracts;
using AutoShortcut.Lib.Contracts.Core;
using AutoShortcut.Lib.Contracts.Media;
using AutoShortcut.Lib.Contracts.Montage;
using AutoShortcut.Lib.Contracts.Services;
using AutoShortcut.Lib.Core;
using AutoShortcut.Lib.Exceptions;
using AutoShortcut.Lib.Montage.Effects;
using AutoShortcut.Lib.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace AutoShortcut.Lib.Montage;

public class MediaTrackCompiler : ITrackCompiler
{
    private readonly IFFmpegEngine _scriptEngine;
    private readonly INameService _nameService;
    private readonly IServiceProvider _services;
    private readonly CompilationStrategy _strategy;
    private IList<object?>? _handlers;
    private readonly ProjectConfig _projectConfig;
    private IMediaFile? _previousHandled;

    public MediaTrackCompiler(
        IFFmpegEngine scriptEngine, 
        IConfigurationProvider configuration, 
        INameService nameService, 
        IServiceProvider services,
        CompilationStrategy strategy)
    {
        _scriptEngine = scriptEngine;
        _nameService = nameService;
        _services = services;
        _strategy = strategy;
        _projectConfig = configuration.GetProjectConfig();
    }
    
    public async Task<IMediaFile> CompileAsync(ITrack track, CancellationToken ctk = default)
    {
        RequireMediaTrack(track);

        foreach (var media in track.Media)
        {
            await ApplyEffectsAsync(media, ctk);
        }

        var script = await _strategy.GenerateScriptAsync(track, ctk);
        var result = await _scriptEngine.ExecuteAsync(script, NewExecutionContext(), ctk);

        return result.Media;
    }

    private static void RequireMediaTrack(ITrack track)
    {
        if (track is not MediaTrack)
            throw new IncorrectValueException($"Current {nameof(MediaTrackCompiler)} process only tracks of type {nameof(MediaTrack)}");
    }

    private async Task ApplyEffectsAsync(IRenderMedia media, CancellationToken ctk = default)
    {
        if (media.Main.AudioAnalyse is null)
        {
            media.AddEffect(new SilentAudioEffect(_services));
        }
        _strategy.BeforeClientEffects(media);

        _previousHandled = media.Main;

        foreach (var lazyEffect in media.Effects)
        {
            IMediaFile handled;
            var effect = lazyEffect.Effect ?? lazyEffect.LoadEffect(_previousHandled);
            
            if (effect is EffectSelfHandler selfHandler)
            {
                handled = await selfHandler.HandleAsync(media, ctk);
            }
            else
            {
                throw new NotImplementedException();
                
                _handlers ??= _services.GetServices(typeof(IEffectHandler<>)).ToList();
                var handler = _handlers.Cast<ITryHandle>().FirstOrDefault(x => x.CanHandle(lazyEffect));

                if (handler is not IEffectHandler<IEffect> realHandler)
                {
                    throw new HandleException($"{nameof(IEffectHandler<IEffect>)} not found to handle media named {media.Main.Name} ({media.Main.FullPath})");
                }
                
                handled = await realHandler.HandleAsync(effect, media, ctk);
            }
            
            media.MarkAsApplied(effect, handled);

            _previousHandled = handled;
        }
        
        _strategy.AfterClientEffects(media);
    }

    private MediaExecutionContext NewExecutionContext()
        => new(MediaStoreType.Personal, _projectConfig.OutputFileName ?? _nameService.NewTemporaryName());
}