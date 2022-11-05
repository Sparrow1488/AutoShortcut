using Microsoft.Extensions.Configuration;
using Sparrow.Video.Abstractions.Services;
using System.Security.Cryptography;

namespace Sparrow.Video.Shortcuts.Services;

public class ReadEncryptedTextFilesService : ReadFileTextService, IReadEncryptedTextFilesService
{
    private readonly IConfiguration _configuration;

    public ReadEncryptedTextFilesService(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public override byte[] OnReadFileBytes(byte[] readed)
    {
        using var aes = Aes.Create();
        var aesKey = _configuration["Security:Keys:Aes256Key"];
        var aesIV = _configuration["Security:Keys:Aes256IV"];
        aes.Key = Convert.FromBase64String(aesKey);
        aes.IV = Convert.FromBase64String(aesIV);
        return aes.DecryptCbc(readed, aes.IV);
    }
}
