using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Processes.Settings
{
    public class VideoFormatSettings : IVideoFormatSettings
    {
        public const string DefaultFileFormat = ".mp4";
        public static readonly Resolution DefaultResolution = Resolution.Small;

        public Resolution DisplayResolution { get; set; } = DefaultResolution;
        /// <summary>
        ///     This is file extensions (.mp4, .avi and other)
        /// </summary>
        public string FileFormat { get; set; } = DefaultFileFormat;
        public FrameFrequency FrameFrequency { get; set; } = FrameFrequency.Fps25;
    }
}
