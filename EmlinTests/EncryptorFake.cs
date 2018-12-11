using Emlin.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmlinTests
{
    class EncryptorFake : IEncryptor
    {
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
