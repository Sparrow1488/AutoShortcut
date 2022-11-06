using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Sparrow.Video.Shortcuts.Environment.Definitions;

public abstract class ApplicationDefinition
{
    public abstract IServiceCollection OnConfigureServices(IServiceCollection services);
}
