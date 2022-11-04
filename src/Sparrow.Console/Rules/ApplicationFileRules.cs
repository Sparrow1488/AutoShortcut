using Sparrow.Video.Abstractions.Enums;

namespace Sparrow.Console.Rules;

public class ApplicationFileRules
{
    /// <summary>
    ///     Приводит видос к общему разрешению в нарезке
    /// </summary>
    public static readonly ScaleFileRule ScaleFileRule = new(Resolution.HD);

    /// <summary>
    ///     Накладывает пустую аудио дорожку на видео, у которого нет звука (нужно для лучшей синхронизации аудио)
    /// </summary>
    public static readonly SilentFileRule SilentFileRule = new();

    /// <summary>
    ///     Используется для корректной конкатинации файлов, конвертируя видеозаписи перед склейкой в кодировку <see cref="EncodingType.Mpegts"/> (.ts)
    /// </summary>
    public static readonly EncodingFileRule EncodingFileRule = new();


    public static readonly LoopMediumFileRule LoopMediumFileRule = new();
    public static readonly LoopShortFileRule LoopShortFileRule = new();
}
