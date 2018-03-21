using System;
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

        public void WriteRecordedDataToFile(KeyCombination[] keyCombinations, string filepath)
        {
            fileSystem.Directory.CreateDirectory(filepath);

            string textToWrite = "";
            foreach (KeyCombination keyComb in keyCombinations)
            {
                textToWrite += AddKeyCombDataLine(textToWrite, keyComb);

            }
            fileSystem.File.AppendAllText(filepath + @"\KeyboardData.txt", textToWrite);
        }

        private static string AddKeyCombDataLine(string textToWrite, KeyCombination keyComb)
        {
            if (keyComb != null)
            {
                textToWrite += String.Format("{0}", keyComb.CombId);

                foreach (TimeSpan time in keyComb.TimeSpanList)
                {
                    textToWrite += String.Format(", {0}", time.Ticks);
                }

                //textToWrite += ";";
                textToWrite += Environment.NewLine;
            }

            return textToWrite;
        }

        public void AddFileSystem(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }
    }
}
