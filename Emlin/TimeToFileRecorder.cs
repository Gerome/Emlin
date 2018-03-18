using System;
using System.IO;
using System.IO.Abstractions;

namespace Emlin
{
    public class TimeToFileRecorder
    {
        readonly IFileSystem fileSystem;

        public TimeToFileRecorder(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;   
        }

        public TimeToFileRecorder() : this(fileSystem: new FileSystem())
        {
        }

        public void WriteRecordedDataToFile(KeyCombination[] keyCombinations, string filepath)
        {
            fileSystem.Directory.CreateDirectory(filepath);

            string textToWrite = "";
            foreach (KeyCombination keyComb in keyCombinations)
            {
               if (keyComb != null)
               {
                    textToWrite += String.Format("{0}", keyComb.CombId);

                    foreach (TimeSpan time in keyComb.TimeSpanList)
                    {
                        textToWrite += String.Format(", {0}", time.Ticks);
                    }

                    textToWrite += ";";
                    textToWrite += Environment.NewLine;
               }

            }
            fileSystem.File.AppendAllText(filepath + @"\KeyboardData.txt", textToWrite);
        }
    }
}
