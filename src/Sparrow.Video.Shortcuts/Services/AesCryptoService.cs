using Microsoft.Extensions.Configuration;
using Sparrow.Video.Abstractions.Services;
using System.Security.Cryptography;

namespace Sparrow.Video.Shortcuts.Services;

public class AesCryptoService : ICryptoService
{
    private readonly IConfiguration _configuration;

    public AesCryptoService(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public byte[] Decrypt(byte[] data)
    {
        using var aes = CreateAes256(out var iv);
        var decrypted = aes.DecryptCbc(data, iv);
        return decrypted;
    }

    public byte[] Encrypt(byte[] data)
    {
        using var aes = CreateAes256(out var iv);
        var encrypted = aes.EncryptCbc(data, iv);
        return encrypted;
    }

    private Aes CreateAes256(out byte[] iv)
    {
        var aes = Aes.Create();
        var aesKey = _configuration["Security:Keys:Aes256Key"];
        var aesIV = _configuration["Security:Keys:Aes256IV"];
        aes.Key = Convert.FromBase64String(aesKey);
        aes.IV = Convert.FromBase64String(aesIV);
        iv = aes.IV;
        return aes;
    }
}
