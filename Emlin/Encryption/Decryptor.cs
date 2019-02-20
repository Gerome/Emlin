using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Emlin.Encryption
{
    public class Decryptor
    {
        public static void DecryptDevFiles()
        {

            DirectoryInfo d = new DirectoryInfo(Environment.CurrentDirectory + @"\..\..\..\Data\Raw");
            FileInfo[] Files = d.GetFiles("KeyboardData_*.txt"); //Getting Text files
            foreach (FileInfo file in Files)
            {
                DecryptFile(file, Environment.CurrentDirectory + @"\..\..\..\Data\Interim\D_" + file.Name);
            }
        }

        public static void DecryptFile(FileInfo file, string targetFilepath)
        {
            IEncryptor decryptor = new Encryptor();

            var lines = File.ReadLines(file.FullName);
            WriteDecryptedLinesToFile(file.Name, targetFilepath, decryptor, lines);
        }

        private static void WriteDecryptedLinesToFile(string fileName, string targetFilepath, IEncryptor decryptor, System.Collections.Generic.IEnumerable<string> lines)
        {
            using (StreamWriter sw = File.CreateText(targetFilepath))
            {
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
