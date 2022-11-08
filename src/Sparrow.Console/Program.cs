using Serilog;
using Sparrow.Console;
using Sparrow.Console.Abstractions;
using Sparrow.Video.Abstractions.Exceptions;
using Sparrow.Video.Shortcuts.Exceptions;
using System.Runtime.InteropServices;

CancellationTokenSource tokenSource = new();
Startup cli = new AutoshortcutStartup();
CancellationToken token = tokenSource.Token;

ChangeConsoleExitStatusCode();
AppDomain.CurrentDomain.ProcessExit += (sender, e) => tokenSource.Cancel();

try
{
    await cli.StartAsync(token);
}
catch (InvalidEnvironmentVariableException ex)
{
    Log.Error(ex.Message);
}
catch (InputResolutionNameNotRequiredException ex)
{
    Log.Error(ex.Message);
}
catch (TaskCanceledException ex)
{
}

void ChangeConsoleExitStatusCode()
{
    SetConsoleCtrlHandler(type =>
    {
        Environment.Exit(-1); // Needs to Invoke AppDomain.CurrentDomain.ProcessExit
        return true;
    }, add: true);
}

[DllImport("kernel32.dll", SetLastError = true)]
static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
delegate bool ConsoleEventDelegate(int eventType);