using AutoShortcut.Lib.Contracts.Services;

namespace AutoShortcut.Lib.Helpful;

public class NameService : INameService
{
    public string NewTemporaryName(string? extension = ".mp4")
    {
        var guid = Guid.NewGuid();
        return guid.ToString()
            .Replace("-", string.Empty)
            .Replace("\\", "/") + extension;
    }
}