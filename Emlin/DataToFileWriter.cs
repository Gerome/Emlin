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
            this.fileSystem = new FileSystem();
        }

        public void WriteRecordedDataToFile(List<KeysData> listOfKeysData, string filepath)
        {
            string textToWrite = "";

            foreach(KeysData data in listOfKeysData)
            {
                textToWrite += GetFormattedDataLine(data);         
            }

            fileSystem.File.WriteAllText(filepath, textToWrite);
            


            //string[] linesInFile = fileSystem.File.ReadAllLines(filepath);
            //int indexOfCurrentKeyComb = 0;
            //KeyCombination currentKeyComb = keyCombinations[indexOfCurrentKeyComb];

            //for (int lineIndex = 0; lineIndex < linesInFile.Length; lineIndex++)
            //{
            //    if (lineIndex == currentKeyComb.CombId)
            //    {
            //        foreach (TimeSpan ts in currentKeyComb.TimeSpanList)
            //        {

            //            linesInFile[lineIndex] += ", " + (ts.TotalMilliseconds).ToString();
            //        }

            //        indexOfCurrentKeyComb++;

            //        if (indexOfCurrentKeyComb != keyCombinations.Count)
            //        {
            //            currentKeyComb = keyCombinations[indexOfCurrentKeyComb];
            //        }
            //        else
            //        {
            //            break;
            //        }
            //    }
            //}

            //fileSystem.File.WriteAllText(filepath, string.Empty);
            //fileSystem.File.WriteAllLines(filepath, linesInFile);
        }

        private string GetFormattedDataLine(KeysData data)
        {
            return data.CombinationID.ToString() + ","
                + data.HoldTime.Milliseconds.ToString() 
                + Environment.NewLine;
        }

        public void CreateDirectoryAndFile(string filepath)
        {
            string directoryPath = Path.GetDirectoryName(filepath);

            

            if (!fileSystem.Directory.Exists(directoryPath))
            {
                fileSystem.Directory.CreateDirectory(directoryPath);
            }

            using (Stream sr = fileSystem.File.Create(filepath)) { }

        }

        public void AddFileSystem(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }
    }
}
