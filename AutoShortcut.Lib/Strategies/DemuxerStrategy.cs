using AutoShortcut.Lib.Contracts;
using AutoShortcut.Lib.Contracts.Core;
using AutoShortcut.Lib.Contracts.Montage;
using AutoShortcut.Lib.Core;
using AutoShortcut.Lib.Helpful;

namespace AutoShortcut.Lib.Strategies;

/// <summary>
/// Использует <see cref="DemuxerCombineMediaScript"/> с перекодировкой видео.
/// Синхронизирует фреймрейт, из-за чего немного херовится конечное качество видео. 
/// </summary>
public class DemuxerStrategy : CompilationStrategy
{
    public override void BeforeClientEffects(IRenderMedia renderMedia) { }
    public override void AfterClientEffects(IRenderMedia renderMedia) { }

    public override Task<Script> GenerateScriptAsync(ITrack track, CancellationToken ctk = default)
    {
        Script script = new DemuxerCombineMediaScript(track.Media.Select(MediaHelper.GetActualFilePath).ToArray());
        return Task.FromResult(script);
    }
}