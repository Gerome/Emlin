using Emlin;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;

namespace EmlinTests
{
    class TimeToFileRecorderTests
    {
        private KeyCombination[] keyCombinations;
        private MockFileSystem fileSystem;
        private TimeToFileRecorder ttfRecorder;

        [SetUp]
        public void Init()
        {
            fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"C:\File\KeyboardData.txt", new MockFileData("") }
            });
            
            ttfRecorder = new TimeToFileRecorder(fileSystem);
        }

        [Test]
        public void CALLING_WRITE_TO_FILE_SHOULD_ADD_CURRENT_DATA_TO_FILE()
        {
            keyCombinations = new KeyCombination[0];
            ttfRecorder.WriteRecordedDataToFile(keyCombinations);

        }
    }
}
