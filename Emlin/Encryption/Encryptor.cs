using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Emlin.Encryption
{
    public class Encryptor : IEncryptor
    {
        public AesCryptoServiceProvider Endec {get; private set;}

        public Encryptor()
        {
            Endec = new AesCryptoServiceProvider
            {
                BlockSize = 128,
                KeySize = 256,
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC,
            };
            Endec.Key = GetKey();
        }

        public string Encrypt(string decrypted)
        {
            Endec.GenerateIV();
            byte[] textbytes = Encoding.ASCII.GetBytes(decrypted);
            ICryptoTransform icrypt = Endec.CreateEncryptor(Endec.Key, Endec.IV);
            byte[] enc = icrypt.TransformFinalBlock(textbytes, 0, textbytes.Length);
            icrypt.Dispose();

            return Convert.ToBase64String(enc);
        }

        public string Decrypted(string lineToDecrypt)
        {
            Endec.IV = Convert.FromBase64String(lineToDecrypt.Split(' ')[0]);
            byte[] textbytes = Convert.FromBase64String(lineToDecrypt.Split(' ')[1]);

            ICryptoTransform icrypt = Endec.CreateDecryptor(Endec.Key, Endec.IV);
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
