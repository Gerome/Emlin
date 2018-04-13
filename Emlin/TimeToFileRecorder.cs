using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;

namespace Emlin
{
    public class TimeToFileRecorder
    {
        IFileSystem fileSystem;

        public TimeToFileRecorder()
        {
            this.fileSystem = new FileSystem();
        }

        public void WriteRecordedDataToFile(List<KeyCombination> keyCombinations, string filepath)
        {
            string[] linesInFile = fileSystem.File.ReadAllLines(filepath);
            int indexOfCurrentKeyComb = 0;
            KeyCombination currentKeyComb = keyCombinations[indexOfCurrentKeyComb];

            for (int lineIndex = 0; lineIndex < linesInFile.Length; lineIndex++)
            {
                if(lineIndex == currentKeyComb.CombId)
                {
                    foreach (TimeSpan ts in currentKeyComb.TimeSpanList)
                    {

                        linesInFile[lineIndex] += ", " + (ts.TotalMilliseconds).ToString();
                    }

                    indexOfCurrentKeyComb++;

                    if (indexOfCurrentKeyComb != keyCombinations.Count)
                    {
                        currentKeyComb = keyCombinations[indexOfCurrentKeyComb];
                    }
                    else
                    {
                        break;
                    }
                }
            }

            fileSystem.File.WriteAllText(filepath, string.Empty);
            fileSystem.File.WriteAllLines(filepath, linesInFile);
        }

        public void CreateDirectoryAndFile(string filepath)
        {
            string directoryPath = Path.GetDirectoryName(filepath);

            if (!fileSystem.Directory.Exists(directoryPath))
            {
                fileSystem.Directory.CreateDirectory(directoryPath);
            }

            if (!fileSystem.File.Exists(filepath))
            {
                fileSystem.File.Create(filepath);
            }
            
        }

        public void PopulateTextFileIfEmpty(string filepath)
        {
            bool fileIsEmpty = fileSystem.FileInfo.FromFileName(filepath).Length.Equals(0); 

            if (fileIsEmpty)
            {
                PopulateTextFileWithIndex(filepath);
            }
        }

        private void PopulateTextFileWithIndex(string filepath)
        {
            string stringToWrite = "";

            for (int i = 0; i < ConstantValues.NUMBER_OF_COOMBINATIONS; i++)
            {
                stringToWrite += i.ToString() + "\r\n";
            }

            fileSystem.File.AppendAllText(filepath, stringToWrite);
        }

        public void AddFileSystem(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }
    }
}
