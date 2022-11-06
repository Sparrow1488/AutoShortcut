using Serilog;
using Sparrow.Console;
using Sparrow.Video.Abstractions.Exceptions;
using Sparrow.Video.Shortcuts.Exceptions;
using System.Runtime.InteropServices;

Startup cli = new AutoshortcutStartup();
CancellationTokenSource tokenSource = new();
CancellationToken token = tokenSource.Token;

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
