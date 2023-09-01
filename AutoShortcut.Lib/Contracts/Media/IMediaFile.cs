namespace AutoShortcut.Lib.Contracts.Media;

public interface IMediaFile : IAnalysed
{
    /// <summary>
    /// Название медиа с расширением 
    /// </summary>
    string Name { get; }
    /// <summary>
    /// Путь до медиа
    /// </summary>
    string Path { get; }
    /// <summary>
    /// Полный путь до медиа
    /// </summary>
    string FullPath { get; }
    /// <summary>
    /// Расширение файла
    /// </summary>
    string Extension { get; }
}