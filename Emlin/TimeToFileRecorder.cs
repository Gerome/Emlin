using System;
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

        public TimeToFileRecorder():this(fileSystem: new FileSystem())
        {}

        public void WriteRecordedDataToFile(KeyCombination[] keyCombinations)
        {
            //throw new NotImplementedException();
        }
    }
}
