using Microsoft.Extensions.DependencyInjection;
using Sparrow.Video.Abstractions.Builders.Formats;
using Sparrow.Video.Abstractions.Services;

namespace Sparrow.Video.Shortcuts.Services
{
    public class ScriptFormatsProvider : IScriptFormatsProvider
    {
        public ScriptFormatsProvider(IServiceProvider services)
        {
            _services = services;
        }

        private readonly IServiceProvider _services;

        public TFormat CreateFormat<TFormat>() where TFormat : IScriptBuilderFormat
        {
            return ActivatorUtilities.CreateInstance<TFormat>(_services);
        }
    }
}