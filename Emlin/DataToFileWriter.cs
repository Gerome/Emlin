using Emlin.Encryption;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Security.Cryptography;

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
            string textToWrite = "";

            foreach(KeysData data in listOfKeysData)
            {
                textToWrite += GetFormattedDataLine(data);         
            }

           

            string encryptedString = encryptor.Encrypt(textToWrite);

            using (StreamWriter sw = fileSystem.File.AppendText(filepath))
            {
                sw.Write(encryptedString);
                sw.Write(encryptor.Decrypted(encryptedString));
            }


        }

        private string GetFormattedDataLine(KeysData data)
        {
            return data.CombinationID.ToString() + "," 
                + data.HoldTime.TotalMilliseconds.ToString() + ","
                + data.FlightTime.TotalMilliseconds.ToString() + ','
                + data.Digraph1.TotalMilliseconds.ToString() + ','
                + data.Digraph2.TotalMilliseconds.ToString() + ','
                + data.Digraph3.TotalMilliseconds.ToString()
                + Environment.NewLine;
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
