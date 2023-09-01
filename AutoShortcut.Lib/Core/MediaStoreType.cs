namespace AutoShortcut.Lib.Core;

/// <summary>
/// Указывает, какой тип хранилища использовать при работе с медиа
/// </summary>
public enum MediaStoreType
{
    /// <summary>
    /// Для обработки медиа использовать временное хранилище
    /// </summary>
    Temporary,
    /// <summary>
    /// Для обработки медиа использовать персональное хранилище
    /// </summary>
    Personal
}