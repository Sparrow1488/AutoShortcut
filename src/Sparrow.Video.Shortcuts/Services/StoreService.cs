using Newtonsoft.Json;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Primitives.Mementos;

namespace Sparrow.Video.Shortcuts.Services
{
    public class StoreService : IStoreService
    {
        public StoreService(IPathsProvider pathsProvider)
        {
            PathsProvider = pathsProvider;
        }

        public IPathsProvider PathsProvider { get; }
        private JsonSerializerSettings _jsonSettings = new()
        {
            Formatting = Formatting.Indented,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full,
            TypeNameHandling = TypeNameHandling.All
        };

        public async Task<TObject> GetObjectAsync<TObject>(string name)
        {
            var filesMetaDirectory = PathsProvider.GetPathFromCurrent(PathName.FilesMeta);
            var metaFiles = Directory.GetFiles(filesMetaDirectory);
            foreach (var metaFilePath in metaFiles)
            {
                var text = await ReadTextAsync(metaFilePath);
                var metaObject = JsonConvert.DeserializeObject<SaveFileMeta>(text, _jsonSettings);
                if (metaObject.Name == name)
                {
                    var objText = await ReadTextAsync(metaObject.OriginalFilePath);
                    return JsonConvert.DeserializeObject<TObject>(objText, _jsonSettings);
                }
            }
            throw new Exception("GET OBJECT EXCEPTION");
        }

        private async Task<string> ReadTextAsync(string filePath)
        {
            using (var sr = new StreamReader(filePath))
            {
                return await sr.ReadToEndAsync();
            }
        }
    }
}
