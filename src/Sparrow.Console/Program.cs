using Serilog;
using Sparrow.Console;
using Sparrow.Video.Abstractions.Exceptions;
using Sparrow.Video.Shortcuts.Exceptions;

Startup cli = new AutoshortcutStartup();
CancellationTokenSource tokenSource = new();
CancellationToken token = tokenSource.Token;

//ConsoleProcess.CancelSourceOnExit(tokenSource);
ConsoleProcess.OnExit(tokenSource);

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

//internal delegate bool ConsoleEventDelegate(int eventType);

//[DllImport("kernel32.dll", SetLastError = true)]
//extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

//void CancelSourceOnExit(CancellationTokenSource cancellationTokenSource)
//{
//    SetConsoleCtrlHandler((eventType) =>
//    {
//        cancellationTokenSource.Cancel();
//        Environment.Exit(-1);
//        return true;
//    }, add: true);
//}
