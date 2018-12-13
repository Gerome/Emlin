using System.Security.Cryptography;

namespace Emlin.Encryption
{
    public interface IEncryptor
    {
        AesCryptoServiceProvider Endec { get; }

        string Encrypt(string decrypted);
        string Decrypted(string encrypted);
    }
}