using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Emlin.Encryption
{
    public class Decryptor
    {
        public static void DecryptDevFiles()
        {
            string dataFilePath = Path.Combine(ConstantValues.KEYBOARD_DATA_FILEPATH, ConstantValues.KEYBOARD_FILE_NAME);
            string decryptedFilePath = Path.Combine(ConstantValues.KEYBOARD_DATA_FILEPATH, "D_" + ConstantValues.KEYBOARD_CSV_FILE_NAME);
            FileInfo file = new FileInfo(dataFilePath);

            DecryptFile(file, decryptedFilePath);
        }

        public static void DecryptFile(FileInfo file, string targetFilepath)
        {
            IEncryptor decryptor = new Encryptor();

            //TODO Check if file exists or not
            var lines = File.ReadLines(file.FullName);
            WriteDecryptedLinesToFile(file.Name, targetFilepath, decryptor, lines);
        }

        private static void WriteDecryptedLinesToFile(string fileName, string targetFilepath, IEncryptor decryptor, System.Collections.Generic.IEnumerable<string> lines)
        {
            using (StreamWriter sw = File.CreateText(targetFilepath))
            {
                sw.Write("Id,HT,FT" + Environment.NewLine);
                byte[] DecryptionKey = Encryptor.GetDecryptKey(Path.Combine(Environment.CurrentDirectory, @"keys.env"));
                foreach (string line in lines)
                {
                    sw.Write(decryptor.Decrypted(line, DecryptionKey));
                }
            }
        }

        private static string GetFileNumber(string fileName)
        {
            return Regex.Match(fileName, @"\d+").Value;
        }
    }
}
