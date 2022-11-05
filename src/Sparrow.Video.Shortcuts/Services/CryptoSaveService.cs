using Sparrow.Video.Abstractions.Services;

namespace Sparrow.Video.Shortcuts.Services;

public class CryptoSaveService : SaveService, ICryptoSaveService
{
    private readonly ICryptoService _cryptoService;

    public CryptoSaveService(
        IPathsProvider pathsProvider,
        ICryptoService cryptoService) 
    : base(pathsProvider)
    {
        _cryptoService = cryptoService;
    }

    protected override byte[] OnSaveBytes(byte[] toSave)
        => _cryptoService.Encrypt(toSave);
}
