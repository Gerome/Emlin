using Emlin.Encryption;
using System;
using System.Security.Cryptography;

namespace EmlinTests
{
    class EncryptorFake : IEncryptor
    {
        public AesCryptoServiceProvider endec => throw new NotImplementedException();

        public string Decrypted(string encrypted)
        {
            return encrypted;
        }

        public string Encrypt(string decrypted)
        {
            return decrypted;
        }
    }
}
