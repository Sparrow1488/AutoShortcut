using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sparrow.Video.Abstractions.Factories;
using Sparrow.Video.Shortcuts.Extensions;

/*
 * This may not work properly because your videos may be of different types (extension, format, encoding, resolution, etc.).
 * For a better example, see the Sparrow.Console project where I apply video rules and processors
 */

var services = Host.CreateDefaultBuilder().ConfigureServices(services =>
    services.AddShortcutDefinision()).Build().Services;

Console.WriteLine("Starting compile...");
var factory = services.GetRequiredService<IShortcutEngineFactory>();
var engine = factory.CreateEngine();
var pipeline = await engine.CreatePipelineAsync(@"C:\Users\USER\Downloads\Telegram Desktop\videos");

var project = pipeline.CreateProject();
var finallyFile = await engine.StartRenderAsync(project);
Console.WriteLine("Completed, " + finallyFile.Path);