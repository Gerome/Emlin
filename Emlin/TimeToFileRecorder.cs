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
            if (!File.Exists(filepath))
            {
                fileSystem.Directory.CreateDirectory(filepath);
            }


            string textToWrite = "";
            foreach (KeyCombination keyComb in keyCombinations)
            {
                AddKeyCombDataLine(ref textToWrite, keyComb);

            }
            fileSystem.File.AppendAllText(filepath, textToWrite);
        }

        private static string AddKeyCombDataLine(ref string textToWrite, KeyCombination keyComb)
        {
            if (keyComb != null)
            {
                textToWrite += String.Format("{0}", keyComb.CombId);

                foreach (TimeSpan time in keyComb.TimeSpanList)
                {
                    textToWrite += String.Format(", {0}", time.Ticks);
                }
               
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
