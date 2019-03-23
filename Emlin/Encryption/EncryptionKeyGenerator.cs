using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Emlin.Encryption
{
    public class EncryptionKeyGenerator
    {
        private const int KEY_LENGTH = 32;

        public void CreateKeyEnvFile(string keyPath)
        {
            using (StreamWriter sw = File.CreateText(keyPath))
            {
                sw.WriteLine("Key={0}", GenerateEncryptionKey());
            }
        }

        private string GenerateEncryptionKey()
        {
            char[] chars =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[KEY_LENGTH];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(KEY_LENGTH);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
    }
}
