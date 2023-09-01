namespace AutoShortcut.Lib.Contracts.Services;

public interface INameService
{
    string NewTemporaryName(string? extension = ".mp4");
}