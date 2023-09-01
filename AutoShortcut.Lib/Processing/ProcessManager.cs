using System.Diagnostics;
using System.Text;
using AutoShortcut.Lib.Contracts.Services;
using AutoShortcut.Lib.Contracts.Services.Primitives;
using AutoShortcut.Lib.Contracts.Services.Results;

namespace AutoShortcut.Lib.Processing;

#region Annotations

// ReSharper disable once AccessToDisposedClosure

#endregion

public class ProcessManager : IProcessManager
{
    public async Task<ProcessOutput> RunAsync(ProcessRunInfo runInfo, CancellationToken ctk = default)
    {
        var info = MapInfo(runInfo);
        
        using var process = new Process { StartInfo = info };
        await using var registration = ctk.Register(() =>
        {
            process.Kill();
        });

        var result = await StartReadingAsync(process, ctk);
        
        await process.WaitForExitAsync(ctk).ConfigureAwait(false);
        registration.Unregister();

        return new ProcessStringOutput(new StringBuilder(result));
    }

    private static ProcessStartInfo MapInfo(ProcessRunInfo info)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = info.FileName,
            Arguments = info.Arguments,
            CreateNoWindow = !info.ShowConsole,
            WorkingDirectory = Directory.GetCurrentDirectory(), // TODO: из настроек
        };

        // HACK: Читать из настроек можно, когда не включена консоль
        if (!info.ShowConsole)
        {
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardInput = true;
        }

        return startInfo;
    }
    
    private static async Task<string> StartReadingAsync(Process process, CancellationToken ctk = default)
    {
        if (process.StartInfo.CreateNoWindow)
        {
            process.EnableRaisingEvents = true;
            process.Start();
            return await process.StandardOutput.ReadToEndAsync(ctk);
        }
        
        process.Start();
        await process.WaitForExitAsync(ctk);
        return string.Empty;
    }
}