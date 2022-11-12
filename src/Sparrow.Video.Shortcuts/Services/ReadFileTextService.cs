using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Primitives;

namespace Sparrow.Video.Shortcuts.Services
{
    public class ReadFileTextService : IReadFileTextService
    {
        public async Task<string> ReadTextAsync(string filePath)
        {
            var filePathValue = StringPath.CreateExists(filePath);
            return await File.ReadAllTextAsync(filePathValue.Value);
        }
    }
}
