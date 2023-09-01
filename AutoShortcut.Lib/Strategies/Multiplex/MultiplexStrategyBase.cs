using System.Text;
using AutoShortcut.Lib.Configuration;
using AutoShortcut.Lib.Contracts;
using AutoShortcut.Lib.Contracts.Core;
using AutoShortcut.Lib.Contracts.Montage;
using AutoShortcut.Lib.Contracts.Services;
using AutoShortcut.Lib.Helpful;
using AutoShortcut.Lib.Media;
using AutoShortcut.Lib.Montage.Effects;

namespace AutoShortcut.Lib.Strategies.Multiplex;

public abstract class MultiplexStrategyBase : CompilationStrategy
{
    private readonly INameService _nameService;
    private readonly StorageConfig _storeConfig;
    private readonly IServiceProvider _services;

    protected MultiplexStrategyBase(INameService nameService, IConfigurationProvider configuration, IServiceProvider services)
    {
        _nameService = nameService;
        _storeConfig = configuration.GetStorageConfig();
        _services = services;
    }

    protected abstract Script CreateScript(ITrack track, string mediaListPath);
    
    public override void BeforeClientEffects(IRenderMedia renderMedia) { }

    public override void AfterClientEffects(IRenderMedia renderMedia)
    {
        renderMedia.AddEffect(new EncodingVideo(new MpegTsEncoding(), _services));
    }

    public override async Task<Script> GenerateScriptAsync(ITrack track, CancellationToken ctk = default)
    {
        var mediaListPath = await MediaHelper.StoreMediaListAsync(track.Media, _storeConfig, _nameService, ctk);
        return CreateScript(track, mediaListPath);        
    }
}