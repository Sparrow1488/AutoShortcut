using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Primitives;
using System.Text;

namespace Sparrow.Video.Shortcuts.Services;

public class ReadFileTextService : IReadFileTextService
{
    public async Task<string> ReadTextAsync(string filePath)
    {
        var filePathValue = StringPath.CreateExists(filePath);
        var fileBytes = await File.ReadAllBytesAsync(filePathValue.Value);
        fileBytes = OnReadFileBytes(fileBytes);
        var result = Encoding.UTF8.GetString(fileBytes);
        return result;
    }

    public virtual byte[] OnReadFileBytes(byte[] readed) 
        => readed;
}
