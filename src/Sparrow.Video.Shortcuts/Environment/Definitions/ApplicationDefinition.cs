using Microsoft.Extensions.DependencyInjection;

namespace Sparrow.Video.Shortcuts.Environment.Definitions
{
    public abstract class ApplicationDefinition
    {
        public abstract IServiceCollection OnConfigureServices(IServiceCollection services);
    }
}
