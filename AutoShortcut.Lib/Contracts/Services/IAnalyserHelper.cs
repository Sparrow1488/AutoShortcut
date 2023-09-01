using AutoShortcut.Lib.Contracts.Media;

namespace AutoShortcut.Lib.Contracts.Services;

public interface IAnalyserHelper
{
    /// <summary>
    /// Анализирует медиа файл. Обновляет анализ-свойства у <see cref="IMediaFile"/>
    /// </summary>
    /// <param name="media">Медиа для анализа</param>
    /// <param name="ctk">Токен отмены</param>
    Task AnalyseMediaAsync(IMediaFile media, CancellationToken ctk = default);
}