using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Emlin.Encryption
{
    public class Encryptor : IEncryptor
    {
        public AesCryptoServiceProvider endec {get; private set;}

        public Encryptor()
        {
            endec = new AesCryptoServiceProvider()
            {
                BlockSize = 128,
                KeySize = 256,
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC
            };
            endec.Key = GetKey();
        }

        public string Encrypt(string decrypted)
        {
            endec.GenerateIV();
            byte[] textbytes = Encoding.ASCII.GetBytes(decrypted);
            ICryptoTransform icrypt = endec.CreateEncryptor(endec.Key, endec.IV);
            byte[] enc = icrypt.TransformFinalBlock(textbytes, 0, textbytes.Length);
            icrypt.Dispose();
            return Convert.ToBase64String(enc);
        }

        public string Decrypted(string encrypted)
        {
            byte[] textbytes = Convert.FromBase64String(encrypted);
            ICryptoTransform icrypt = endec.CreateDecryptor(endec.Key, endec.IV);
            byte[] enc = icrypt.TransformFinalBlock(textbytes, 0, textbytes.Length);
            icrypt.Dispose();
            return Encoding.ASCII.GetString(enc);
        }

        public static byte[] GetKey()
        {
            DotNetEnv.Env.Load(Directory.GetCurrentDirectory() + "/keys.env");
            return Encoding.ASCII.GetBytes(DotNetEnv.Env.GetString("Key"));
        }
    }
}
