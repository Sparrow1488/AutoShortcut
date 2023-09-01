namespace Sparrow.Video.Abstractions.Services;

public interface IReadFileTextService
{
    Task<string> ReadTextAsync(string filePath);
}