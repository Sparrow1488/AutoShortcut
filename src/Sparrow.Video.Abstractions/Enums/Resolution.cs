using Newtonsoft.Json;
using Sparrow.Video.Abstractions.Exceptions;

namespace Sparrow.Video.Abstractions.Enums
{
    /// <summary>
    ///     Video resolution
    /// </summary>
    [Serializable]
    public class Resolution
    {
        [JsonConstructor]
        private Resolution(string value, int height, int width)
        {
            Value = value;
            Height = height;
            Width = width;
        }

        [JsonProperty]
        public string Value { get; }
        [JsonProperty]
        public int Height { get; }
        [JsonProperty]
        public int Width { get; }

        /// <summary>
        ///     Display resoulution in 360p
        /// </summary>
        public static readonly Resolution Preview = new("360p", 360, 640);
        /// <summary>
        ///     Display resoulution in 480p
        /// </summary>
        public static readonly Resolution Small   = new("480p", 480, 854);
        /// <summary>
        ///     Display resoulution in 480p
        /// </summary>
        public static readonly Resolution HD      = new("720p", 720, 1280);
        /// <summary>
        ///     Display resoulution in 1080p
        /// </summary>
        public static readonly Resolution FHD     = new("1080p", 1080, 1920);
        /// <summary>
        ///     Display resoulution in 2K (1440p)
        /// </summary>
        public static readonly Resolution QHD     = new("1440p", 1440, 2560);
        /// <summary>
        ///     Display resoulution in 4K Ultra HD (2160p)
        /// </summary>
        public static readonly Resolution UHD     = new("2160p", 2160, 3840);

        public static Resolution ParseRequiredResolution(string name)
        {
            var resolution = ParseResolution(name);
            return resolution ?? throw new InputResolutionNameNotRequiredException(
                    $"Invalid input resolution '{name}'");
        }

        public static Resolution? ParseResolution(string name)
        {
            var fields = typeof(Resolution).GetFields();
            var resolution = fields.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
            if (resolution is not null)
            {
                var value = resolution.GetValue(null);
                return (Resolution)value!;
            }
            return null;
        }
    }
}
