namespace Sparrow.Video.Abstractions.Processes.Settings
{
    public interface IEncodingSettings
    {
        /// <summary>
        ///     One of <see cref="Enums.EncodingType"/>
        /// </summary>
        string EncodingType { get; }
    }
}
