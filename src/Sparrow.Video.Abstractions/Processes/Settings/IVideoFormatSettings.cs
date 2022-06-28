using Sparrow.Video.Abstractions.Enums;

namespace Sparrow.Video.Abstractions.Processes.Settings
{
    public interface IVideoFormatSettings : IFormatSettings
    {
        Resolution DisplayResolution { get; }
    }
}
