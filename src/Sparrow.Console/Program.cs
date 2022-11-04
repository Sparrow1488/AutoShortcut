using Serilog;
using Sparrow.Console;
using Sparrow.Video.Abstractions.Exceptions;
using Sparrow.Video.Shortcuts.Exceptions;

Startup cli = new AutoshortcutStartup();
CancellationToken token = new();

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