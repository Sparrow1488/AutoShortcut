using Sparrow.Video.Abstractions.Services;

namespace Sparrow.Video.Shortcuts.Services;

public class ReadEncryptedTextFilesService : ReadFileTextService, IReadEncryptedTextFilesService
{
    private readonly ICryptoService _cryptoService;

    public ReadEncryptedTextFilesService(
        ICryptoService cryptoService)
    {
        _cryptoService = cryptoService;
    }

    public override byte[] OnReadFileBytes(byte[] readed)
        => _cryptoService.Decrypt(readed);
}
