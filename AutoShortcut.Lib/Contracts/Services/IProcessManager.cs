using AutoShortcut.Lib.Contracts.Services.Primitives;
using AutoShortcut.Lib.Contracts.Services.Results;

namespace AutoShortcut.Lib.Contracts.Services;

public interface IProcessManager
{
    Task<ProcessOutput> RunAsync(ProcessRunInfo runInfo, CancellationToken ctk = default);
}