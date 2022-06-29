using Microsoft.Extensions.DependencyInjection;
using Sparrow.Video.Abstractions.Enginies;
using Sparrow.Video.Abstractions.Factories;

namespace Sparrow.Video.Shortcuts.Factories
{
    public class ShortcutEngineFactory : IShortcutEngineFactory
    {
        public ShortcutEngineFactory(IServiceProvider services)
        {
            Services = services;
        }

        public IServiceProvider Services { get; private set; }

        public IShortcutEngine CreateEngine()
        {
            var engine = Services.GetService<IShortcutEngine>();
            return engine;
        }
    }
}
