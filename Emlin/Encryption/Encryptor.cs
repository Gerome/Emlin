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
        }

        public string Encrypt(string unencrypted)
        {
            Endec.Key = GetKey();
            Endec.GenerateIV();
            byte[] textbytes = Encoding.ASCII.GetBytes(unencrypted);
            ICryptoTransform icrypt = Endec.CreateEncryptor(Endec.Key, Endec.IV);
            byte[] enc = icrypt.TransformFinalBlock(textbytes, 0, textbytes.Length);
            icrypt.Dispose();

            return Convert.ToBase64String(enc);
        }

        public string Decrypted(string lineToDecrypt, string fileNumber)
        {
            Endec.Key = GetDecryptKey(fileNumber);
            Endec.IV = Convert.FromBase64String(lineToDecrypt.Split(' ')[0]);
            byte[] textbytes = Convert.FromBase64String(lineToDecrypt.Split(' ')[1]);

            ICryptoTransform icrypt = Endec.CreateDecryptor(Endec.Key, Endec.IV);
            byte[] enc = icrypt.TransformFinalBlock(textbytes, 0, textbytes.Length);
            icrypt.Dispose();
            return Encoding.ASCII.GetString(enc);
        }

        private byte[] GetDecryptKey(string fileNumber)
        {
            DotNetEnv.Env.Load(Environment.CurrentDirectory +  @"..\..\..s\Keys\keys_" + fileNumber + ".env");
            return Encoding.ASCII.GetBytes(DotNetEnv.Env.GetString("Key"));
        }

        public static byte[] GetKey()
        {
            DotNetEnv.Env.Load(Directory.GetCurrentDirectory() + "/keys.env");
            return Encoding.ASCII.GetBytes(DotNetEnv.Env.GetString("Key"));
        }
    }
}
