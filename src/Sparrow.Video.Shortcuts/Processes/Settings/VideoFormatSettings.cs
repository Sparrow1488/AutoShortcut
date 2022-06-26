using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Processes.Settings
{
    public class VideoFormatSettings : IVideoFormatSettings
    {
        public const string DefaultFileFormat = ".mp4";
        public static readonly Resolution DefaultResolution = Resolution.Small;

        public Resolution DisplayResolution { get; set; } = DefaultResolution;
        public ISaveSettings SaveSettings { get; set; } = new SaveSettings();
        public string FileFormat { get; set; } = DefaultFileFormat;
    }
}
