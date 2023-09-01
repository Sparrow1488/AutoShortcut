using AutoShortcut.Lib.Contracts.Core;
using AutoShortcut.Lib.Contracts.Media;
using AutoShortcut.Lib.Contracts.Services;
using AutoShortcut.Lib.Core;
using AutoShortcut.Lib.Helpful;
using Microsoft.Extensions.DependencyInjection;

namespace AutoShortcut.Lib.Montage.Effects;

public class LoopVideo : EffectSelfHandler<LoopVideo>, IVideoEffect
{
    private readonly IServiceProvider _services;

    public LoopVideo(int loopCount, IServiceProvider services) : base(services)
    {
        _services = services;
        LoopCount = loopCount;
    }
    
    private int LoopCount { get; }

    protected override Script NewScript(IMediaFile changing)
        => throw new NotImplementedException("Use async override");

    protected override async Task<Script> NewScriptAsync(IMediaFile changing, CancellationToken ctk = default)
    {
        var nameService = _services.GetRequiredService<INameService>();
        var storeConfig = _services.GetRequiredService<IConfigurationProvider>().GetStorageConfig();

        var loopPath = Enumerable.Range(1, LoopCount).Select(_ => changing.Path).ToArray();
        var listPath = await MediaHelper.StoreMediaListAsync(loopPath, storeConfig, nameService, ctk);

        return new LoopVideoDemuxerScript(listPath);
    }
}