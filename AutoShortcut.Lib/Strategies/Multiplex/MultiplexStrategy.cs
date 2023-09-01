using AutoShortcut.Lib.Contracts;
using AutoShortcut.Lib.Contracts.Core;
using AutoShortcut.Lib.Contracts.Montage;
using AutoShortcut.Lib.Contracts.Services;
using AutoShortcut.Lib.Core;
using AutoShortcut.Lib.Media;
using AutoShortcut.Lib.Montage.Effects;

namespace AutoShortcut.Lib.Strategies.Multiplex;

/// <summary>
/// Использует <see cref="MultiplexCombineMediaScript"/> скрипт без перекодирования.
/// Для успешного использования все видео должны быть с одинаковыми параметрами (высота, ширина, фреймрейт, фпс и тд).
/// Пока с этим проблемы, однако видео мгновенно склеивается.
/// </summary>
public class MultiplexStrategy : MultiplexStrategyBase
{
    private readonly IServiceProvider _services;

    public MultiplexStrategy(
        INameService nameService, 
        IConfigurationProvider configuration, 
        IServiceProvider services) 
    : base(nameService, configuration, services)
    {
        _services = services;
    }
    
    public override void AfterClientEffects(IRenderMedia renderMedia)
    {
        renderMedia.AddEffect(new EncodingVideo(new MpegTsEncoding(), _services));
    }

    protected override Script CreateScript(ITrack track, string mediaListPath)
    {
        return new MultiplexCombineMediaScript(mediaListPath);   
    }
}