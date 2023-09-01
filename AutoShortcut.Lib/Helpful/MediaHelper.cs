using System.Text;
using AutoShortcut.Lib.Configuration;
using AutoShortcut.Lib.Contracts.Media;
using AutoShortcut.Lib.Contracts.Montage;
using AutoShortcut.Lib.Contracts.Services;

namespace AutoShortcut.Lib.Helpful;

public static class MediaHelper
{
    public static string GetActualFilePath(IRenderMedia file)
        => file.LastChange is not null
            ? ((IMediaFile) file.LastChange.Source).Path
            : file.Main.Path;

    /// <summary>
    /// Сохраняет список медиа в файле
    /// </summary>
    /// <returns>Путь до файла</returns>
    public static Task<string> StoreMediaListAsync(
        IEnumerable<IRenderMedia> mediaFiles,
        StorageConfig storeConfig,
        INameService nameService,
        CancellationToken ctk = default)
    {
        var content = new StringBuilder();

        foreach (var file in mediaFiles)
        {
            content.AppendLine($"file '{Path.GetFullPath(GetActualFilePath(file))}'");
        }

        return InternalStoreMediaListAsync(content, storeConfig, nameService, ctk);
    }
    
    /// <summary>
    /// Сохраняет список медиа в файле
    /// </summary>
    /// <returns>Путь до файла</returns>
    public static Task<string> StoreMediaListAsync(
        IEnumerable<string> filesPath,
        StorageConfig storeConfig,
        INameService nameService,
        CancellationToken ctk = default)
    {
        var content = new StringBuilder();

        foreach (var file in filesPath)
        {
            content.AppendLine($"file '{Path.GetFullPath(file)}'");
        }

        return InternalStoreMediaListAsync(content, storeConfig, nameService, ctk);
    }

    private static async Task<string> InternalStoreMediaListAsync(
        StringBuilder builder, 
        StorageConfig storeConfig, 
        INameService nameService,
        CancellationToken ctk = default)
    {
        var path = Path.GetFullPath(storeConfig.TemporaryFilePath(nameService.NewTemporaryName(".txt")));
        await File.WriteAllTextAsync(path, builder.ToString(), ctk);

        return path;
    }
}