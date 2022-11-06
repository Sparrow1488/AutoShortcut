using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Primitives;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Processes.Results;
using Sparrow.Video.Shortcuts.Processes.Settings;
using System.Diagnostics;

namespace Sparrow.Video.Shortcuts.Processes;

public abstract class ExecutionProcessBase : IExecutionProcess
{
    public ExecutionProcessBase(
        IConfiguration configuration,
        ILogger<ExecutionProcessBase> logger)
    {
        Configuration = configuration;
        Logger = logger;
    }

    protected ITextProcessResult TextProcessResult { get; private set; }
    public IConfiguration Configuration { get; }
    public ILogger<ExecutionProcessBase> Logger { get; }

    protected virtual Task OnStartingProcessAsync(ProcessSettings processSettings, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public virtual async Task StartAsync(CancellationToken cancellationToken = default)
    {
        string result = "";
        var settings = OnConfigureSettings();
        var startInfo = CreateStartInfo();

        startInfo.Arguments = settings.Argument;
        using (var process = new Process() { StartInfo = startInfo })
        {
            cancellationToken.Register(() =>
            {
                Logger.LogInformation("CANCELLATION TOKEN KILLED THE TASK");
                process.Kill();
            });

            await OnStartingProcessAsync(settings, cancellationToken);
            if (settings.IsReadOutputs)
            {
                result = await StartReadingAsync(process);
            }
            else
            {
                process.Start();
            }
            await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
        }
        TextProcessResult = new TextProcessResult(settings, result);
    }

    protected abstract StringPath OnGetProcessPath();
    protected virtual ProcessSettings OnConfigureSettings()
    {
        var settings = new ProcessSettings();
        if (Configuration.IsDebug())
        {
            settings.IsShowConsole = true;
        }
        return settings;
    }

    private ProcessStartInfo CreateStartInfo()
    {
        var startInfo = new ProcessStartInfo()
        {
            FileName = OnGetProcessPath().Value,
            WorkingDirectory = Directory.GetCurrentDirectory(),
            CreateNoWindow = true
        };
        var settings = OnConfigureSettings();
        if (settings.IsReadOutputs)
        {
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardInput = true;
            startInfo.UseShellExecute = false;
        }
        if (settings.IsShowConsole)
        {
            startInfo.CreateNoWindow = false;
        }
        return startInfo;
    }

    private async Task<string> StartReadingAsync(Process process)
    {
        process.EnableRaisingEvents = true;
        process.Start();
        return await process.StandardOutput.ReadToEndAsync();
    }
}
