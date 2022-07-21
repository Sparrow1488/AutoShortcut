namespace Sparrow.Video.Abstractions.Enums
{
    /// <summary>
    ///     Video resolution
    /// </summary>
    public class Resolution
    {
        private Resolution(string resolution, int height, int width)
        {
            Value = resolution;
            Height = height;
            Width = width;
        }

        public string Value { get; }
        public int Height { get; }
        public int Width { get; }

        /// <summary>
        ///     Display resoulution in 480p
        /// </summary>
        public static readonly Resolution Small = new Resolution("480p", 480, 640);
        /// <summary>
        ///     Display resoulution in 480p
        /// </summary>
        public static readonly Resolution HD = new Resolution("720p", 720, 1280);
        /// <summary>
        ///     Display resoulution in 1080p
        /// </summary>
        public static readonly Resolution FHD = new Resolution("1080p", 1080, 1920);
        /// <summary>
        ///     Display resoulution in 2K (1440p)
        /// </summary>
        public static readonly Resolution QHD = new Resolution("1440p", 1440, 2560);
        /// <summary>
        ///     Display resoulution in 4K Ultra HD (2160p)
        /// </summary>
        public static readonly Resolution UHD = new Resolution("2160p", 2160, 3840);
    }
}
