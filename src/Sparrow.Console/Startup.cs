using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Sparrow.Console.Rules;
using Sparrow.Video.Abstractions.Factories;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Primitives;
using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Exceptions;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Primitives.Structures;

namespace Sparrow.Console;
internal abstract class Startup
{
    public Startup()
    {
        var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.Console.json").Build();
        string filesDirectory = configuration["FilesRootDirectoryPath"];
        FilesDirectoryPath = StringPath.CreateExists(filesDirectory);
    }

    public StringPath FilesDirectoryPath { get; }
    public IServiceProvider ServiceProvider { get; set; } = default!;
    public IEnvironmentVariablesProvider Variables { get; set; } = default!;

    protected void FixNames()
    {
        var files = Directory.GetFiles(FilesDirectoryPath.Value);
        foreach (var filePath in files)
        {
            var newName = Path.GetFileName(filePath).Replace("'", "");
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

    public virtual void OnConfigreDevelopmentVariables(IEnvironmentVariablesProvider variables) { }

    public abstract Task OnStart(CancellationToken cancellationToken = default);
    
    private void OnConfigureStartup()
    {
        ServiceProvider = Host.CreateDefaultBuilder()
            .UseSerilog((context, services, configuration) => configuration
                .Enrich.FromLogContext()
                .WriteTo.Console())
            .ConfigureServices(services =>
            {
                services.AddShortcutDefinision();
            })
            .Build().Services;

        Variables = ServiceProvider.GetRequiredService<IEnvironmentVariablesProvider>();
        if (Variables.IsDevelopment())
        {
            OnConfigreDevelopmentVariables(Variables);
        }
    }
}
