namespace Sparrow.Video.Abstractions.Services;

public interface ICryptoService
{
    byte[] Decrypt(byte[] data);
    byte[] Encrypt(byte[] data);
}
