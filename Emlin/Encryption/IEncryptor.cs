using System.Security.Cryptography;

namespace Emlin.Encryption
{
    public interface IEncryptor
    {
        AesCryptoServiceProvider Endec { get; }

        string Encrypt(string unencrypted);
        string Decrypted(string encrypted, string fileNumber);
    }
}