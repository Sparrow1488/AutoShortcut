using AutoShortcut.Lib.Core;

namespace AutoShortcut.Lib.Contracts.Core;

public interface IScriptEngine<TResult, in TContext>
where TResult : ScriptExecutionResult
where TContext : ExecutionContext
{
    Task<TResult> ExecuteAsync(Script script, TContext context, CancellationToken ctk = default);
}

public interface IFFmpegEngine : IScriptEngine<MediaScriptResult, MediaExecutionContext>
{
    
}