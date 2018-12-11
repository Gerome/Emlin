namespace Emlin.Encryption
{
    public interface IEncryptor
    {
        string Encrypt(string decrypted);
        string Decrypted(string encrypted);
    }
}