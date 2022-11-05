using Microsoft.Extensions.Configuration;
using Sparrow.Video.Abstractions.Services;
using System.Security.Cryptography;

namespace Sparrow.Video.Shortcuts.Services;

public class CryptoSaveService : SaveService, ICryptoSaveService
{
    private readonly IConfiguration _configuration;

    public CryptoSaveService(
        IPathsProvider pathsProvider,
        IConfiguration configuration) 
    : base(pathsProvider)
    {
        _configuration = configuration;
    }

    protected override byte[] OnSaveBytes(byte[] toSave)
    {
        using var aes = Aes.Create();
        var aesKey = _configuration["Security:Keys:Aes256Key"];
        var aesIV = _configuration["Security:Keys:Aes256IV"];
        aes.Key = Convert.FromBase64String(aesKey);
        aes.IV = Convert.FromBase64String(aesIV);
        toSave = aes.EncryptCbc(toSave, aes.IV);
        return toSave;
    }
}
