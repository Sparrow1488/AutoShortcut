using Sparrow.Video.Abstractions.Builders.Formats;

namespace Sparrow.Video.Abstractions.Services
{
    public interface IScriptFormatsProvider
    {
        TFormat CreateFormat<TFormat>()
            where TFormat : IScriptBuilderFormat;
    }
}
