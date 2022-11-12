using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Primitives;
using Sparrow.Video.Shortcuts.Exceptions;
using Sparrow.Video.Shortcuts.Extensions;

namespace Sparrow.Console.Abstractions;
internal abstract class Startup
{
    protected StringPath FilesDirectoryPath { get; private set; }
    protected IServiceProvider ServiceProvider { get; set; } = default!;
    protected IEnvironmentVariablesProvider Variables { get; set; } = default!;

    private void FixNames()
    {
        var files = Directory.GetFiles(FilesDirectoryPath.Value);
        foreach (var filePath in files)
        {
            var newName = Path.GetFileName(filePath).Replace("'", "")
                                                         .Replace("%", "");
            var newPath = Path.Combine(Path.GetDirectoryName(filePath)!, newName);
            File.Move(filePath, newPath);
        }
    }

    public async Task StartAsync(CancellationToken token = default!)
    {
        OnConfigureStartup();
        FixNames();
        await OnStart(token);
    }

    protected virtual void OnConfigureDevelopmentVariables(IEnvironmentVariablesProvider variables) { }

    protected abstract Task OnStart(CancellationToken cancellationToken = default);

    private void OnConfigureStartup()
    {
        ServiceProvider = Host.CreateDefaultBuilder()
            .UseSerilog((context, services, configuration) => configuration
                .Enrich.FromLogContext()
                .MinimumLevel.Information()
                .WriteTo.Console())
            .ConfigureServices(services =>
            {
                services.AddShortcutDefinision(services.BuildServiceProvider());
            })
            .Build().Services;

        Variables = ServiceProvider.GetRequiredService<IEnvironmentVariablesProvider>();
        if (Variables.IsDevelopment())
        {
            OnConfigureDevelopmentVariables(Variables);
        }

        var inputDirectoryPath = Variables.GetInputDirectoryPath()
                                    ?? throw new InvalidEnvironmentVariableException(
                                        "No specified files directory path. Use '-input' variable");
        FilesDirectoryPath = StringPath.CreateExists(inputDirectoryPath);
    }
}
