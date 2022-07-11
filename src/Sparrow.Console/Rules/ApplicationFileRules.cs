using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Shortcuts.Extensions;

namespace Sparrow.Console.Rules
{
    public class ApplicationFileRules
    {
        /// <summary>
        ///     Приводит видос к общему разрешению в нарезке
        /// </summary>
        public static readonly ScaleFileRule ScaleFileRule = new(
            Resolution.FHD, file => true);

        public static readonly LoopFileRule LoopMediumFileRule = new(
            file => file.Analyse.StreamAnalyses.Video().Duration < 14 && 
                    file.Analyse.StreamAnalyses.Video().Duration > 8) {
            LoopCount = 2
        };

        public static readonly LoopFileRule LoopShortFileRule = new(
            file => file.Analyse.StreamAnalyses.Video().Duration <= 8) {
            LoopCount = 3
        };

        /// <summary>
        ///     Накладывает пустую аудио дорожку на видео, у которого нет звука 
        ///     (нужно для лучшей синхронизации аудио)
        /// </summary>
        public static readonly SilentFileRule SilentFileRule = new(
            file => !file.Analyse.StreamAnalyses.WithAudio());

        /// <summary>
        ///     Используется для корректной конкатинации файлов
        /// </summary>
        public static readonly EncodingFileRule EncodingFileRule = new(file => true)
        {
            EncodingType = EncodingType.Mpegts
        };
    }
}
