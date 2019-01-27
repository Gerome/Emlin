using Emlin.Encryption;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;

namespace Emlin
{
    public class DataToFileWriter
    {
        IFileSystem fileSystem;

        public DataToFileWriter()
        {
            fileSystem = new FileSystem();
        }

        public void WriteRecordedDataToFile(List<KeysData> listOfKeysData, string filepath, IEncryptor encryptor)
        {
            List<string> textToWrite = new List<string>();

            foreach(KeysData data in listOfKeysData)
            {
                textToWrite.Add(DataFormatter.GetFormattedDataLine(data)+ Environment.NewLine);         
            }

          
            using (StreamWriter sw = fileSystem.File.AppendText(filepath))
            {
                foreach(string line in textToWrite)
                {
                    string encryptedLine = encryptor.Encrypt(line);
                    WriteEncryptorIV(encryptor, sw);
                    sw.Write(encryptedLine + Environment.NewLine);
                }
            }
         }

        private static void WriteEncryptorIV(IEncryptor encryptor, StreamWriter sw)
        {
            sw.Write(Convert.ToBase64String(encryptor.Endec.IV) + " ");
        }

        

        public void CreateDirectoryAndFile(string filepath)
        {
            string directoryPath = Path.GetDirectoryName(filepath);

            

            if (!fileSystem.Directory.Exists(directoryPath))
            {
                fileSystem.Directory.CreateDirectory(directoryPath);
            }

            using (StreamWriter sw = fileSystem.File.AppendText(filepath)) { }

        }

        public void AddFileSystem(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }
    }
}
