using AutoShortcut.Lib.Contracts;
using AutoShortcut.Lib.Contracts.Core;
using AutoShortcut.Lib.Contracts.Montage;

namespace AutoShortcut.Lib.Strategies;

public abstract class CompilationStrategy
{
    public abstract void BeforeClientEffects(IRenderMedia renderMedia);
    public abstract void AfterClientEffects(IRenderMedia renderMedia);
    public abstract Task<Script> GenerateScriptAsync(ITrack track, CancellationToken ctk = default);
}