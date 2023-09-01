using AutoShortcut.Lib.Contracts;
using AutoShortcut.Lib.Contracts.Core;
using AutoShortcut.Lib.Contracts.Services;
using AutoShortcut.Lib.Core;

namespace AutoShortcut.Lib.Strategies.Multiplex;

/// <summary>
/// Использует <see cref="MultiplexTimestampCombineMediaScript"/> с перекодировкой.
/// Как то фиксит timestamp, однако разницы с <see cref="MultiplexStrategy"/> особой не вижу, если параметры видео разные.
/// </summary>
public class MultiplexTimestampStrategy : MultiplexStrategyBase
{
    public MultiplexTimestampStrategy(INameService nameService, IConfigurationProvider configuration, IServiceProvider services) : base(nameService, configuration, services)
    {
    }

    protected override Script CreateScript(ITrack track, string mediaListPath)
    {
        return new MultiplexTimestampCombineMediaScript(mediaListPath);
    }
}