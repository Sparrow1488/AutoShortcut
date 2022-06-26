using Newtonsoft.Json;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Abstractions.Services.Options;
using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Primitives.Mementos;
using System.Text;

namespace Sparrow.Video.Shortcuts.Services
{
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

        // TODO: save options (Имя и id) - это мета,
        // по которой можно найти файл, который может храниться отдельно.
        // Нужно сделать возможность настройки пути, где хранится мета
        public async Task SaveAsync<TObject>(
            TObject @object, 
            ISaveOptions saveOptions, 
            CancellationToken cancellationToken = default)
        {
            var serializedObject = JsonConvert.SerializeObject(@object, _jsonSettings);
            var fileName = Path.GetRandomFileName();
            var fullFilePath = Path.Combine(saveOptions.DirectoryPath, fileName);
            var fullFilePathWithExtension = fullFilePath + ".json";
            await SaveObjectAsync(fullFilePathWithExtension, serializedObject, cancellationToken);

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
            await SaveObjectAsync(metaFileFullName, fileMeta, cancellationToken);
        }

        private async Task SaveObjectAsync(
            string fullFilePath, string serializedObject, CancellationToken cancellation)
        {
            var fileDirectory = new FileInfo(fullFilePath).Directory.FullName;
            Directory.CreateDirectory(fileDirectory);
            using (var fileStream = File.Create(fullFilePath))
            {
                var objectBytes = EncodingString(serializedObject, Encoding.UTF8);
                await fileStream.WriteAsync(objectBytes, cancellation);
            }
        }

        private byte[] EncodingString(string obj, Encoding encoding)
        {
            return encoding.GetBytes(obj);
        }
    }
}
