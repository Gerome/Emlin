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
                textToWrite.Add(GetFormattedDataLine(data));         
            }

          
            using (StreamWriter sw = fileSystem.File.AppendText(filepath))
            {
                foreach(string line in textToWrite)
                {
#if DEBUG
                    sw.Write(line);
#else
                    WriteEncryptorIV(encryptor, sw);
                    string encryptedLine = encryptor.Encrypt(line);
                    sw.Write(encryptedLine + Environment.NewLine);
#endif
                }
            }
        }

        private static void WriteEncryptorIV(IEncryptor encryptor, StreamWriter sw)
        {
            sw.Write(Convert.ToBase64String(encryptor.Endec.IV) + " ");
        }

        private string GetFormattedDataLine(KeysData data)
        {
            return data.CombinationID.ToString() + "," 
                + ((int)data.HoldTime.TotalMilliseconds).ToString() + ","
                + ((int)data.FlightTime.TotalMilliseconds).ToString() + ','
                + ((int)data.Digraph1.TotalMilliseconds).ToString() + ','
                + ((int)data.Digraph2.TotalMilliseconds).ToString() + ','
                + ((int)data.Digraph3.TotalMilliseconds).ToString()
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
