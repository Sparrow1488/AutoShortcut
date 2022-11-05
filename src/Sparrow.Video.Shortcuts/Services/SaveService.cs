using Newtonsoft.Json;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Abstractions.Services.Options;
using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Primitives.Mementos;
using System.Text;

namespace Sparrow.Video.Shortcuts.Services;

public class SaveService : ISaveService
{
    public SaveService(IPathsProvider pathsProvider)
    {
        _pathsProvider = pathsProvider;
    }

    private readonly JsonSerializerSettings _jsonSettings = new()
    {
        Formatting = Formatting.Indented,
        TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full,
        TypeNameHandling = TypeNameHandling.All
    };
    private readonly IPathsProvider _pathsProvider;

    public async Task SaveJsonAsync<TObject>(
        TObject @object, 
        ISaveOptions saveOptions, 
        CancellationToken cancellationToken = default)
    {
        var serializedObject = JsonConvert.SerializeObject(@object, _jsonSettings);
        var fileName = Path.GetRandomFileName();
        var fullFilePath = Path.Combine(saveOptions.DirectoryPath, fileName);
        var fullFilePathWithExtension = fullFilePath + ".json";
        await SaveTextAsync(fullFilePathWithExtension, serializedObject, cancellationToken);

        var metaMemento = new SaveFileMeta()
        {
            Id = saveOptions.Id,
            Name = saveOptions.Name,
            DirectoryPath = saveOptions.DirectoryPath,
            OriginalFilePath = fullFilePathWithExtension
        };
        var metaFilesPath = _pathsProvider.GetPathFromCurrent(PathName.FilesMeta);
        var fileMeta = JsonConvert.SerializeObject(metaMemento, _jsonSettings);
        var metaFileFullName = Path.Combine(metaFilesPath, $"{fileName}.meta.json");
        await SaveTextAsync(metaFileFullName, fileMeta, cancellationToken);
    }

    public async Task SaveTextAsync(
        string text, ISaveSettings saveSettings, CancellationToken cancellationToken = default)
    {
        await SaveTextAsync(saveSettings.SaveFullPath, text, cancellationToken);
    }

    private async Task SaveTextAsync(
        string fullFilePath, string saveText, CancellationToken cancellation)
    {
        var fileDirectory = new FileInfo(fullFilePath).Directory.FullName;
        Directory.CreateDirectory(fileDirectory);
        using var fileStream = File.Create(fullFilePath);
        var objectBytes = EncodingString(saveText, Encoding.UTF8);
        objectBytes = OnSaveBytes(objectBytes);
        await fileStream.WriteAsync(objectBytes, cancellation);
    }

    private static byte[] EncodingString(string obj, Encoding encoding)
        => encoding.GetBytes(obj);

    protected virtual byte[] OnSaveBytes(byte[] toSave)
        => toSave;
}
