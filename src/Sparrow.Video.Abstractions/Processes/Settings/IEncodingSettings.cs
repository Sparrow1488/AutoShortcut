namespace Sparrow.Video.Abstractions.Processes.Settings
{
    public interface IEncodingSettings
    {
        /// <summary>
        ///     One of <see cref="Enums.EncodingType"/>
        /// </summary>
        string EncodingType { get; }
        /// <summary>
        ///     Where need to save encoded file
        /// </summary>
        ISaveSettings SaveSettings { get; }
    }
}
