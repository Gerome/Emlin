using Emlin;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;

namespace EmlinTests
{
    class TimeToFileRecorderTests
    {
        private KeyCombination[] keyCombinations;
        private MockFileSystem fileSystem = new MockFileSystem();
        private TimeToFileRecorder ttfRecorder;

        [SetUp]
        public void Init()
        {
            fileSystem.AddDirectory(@"D:\File\");
            fileSystem.AddFile(@"KeyboardData.txt", "");
            ttfRecorder = new TimeToFileRecorder(fileSystem);
        }

        [Test]
        public void CALLING_WRITE_TO_FILE_SHOULD_ADD_CURRENT_DATA_TO_FILE()
        {
            KeyCombination keyComb = new KeyCombination(0);

            keyCombinations = new KeyCombination[1];
            keyComb.AddTimespanToList(new TimeSpan(300000));

            keyCombinations[0] = keyComb;

            ttfRecorder.WriteRecordedDataToFile(keyCombinations, @"D:\File\");

            Assert.That(fileSystem.GetFile(@"D:\File\KeyboardData.txt").TextContents, Does.Contain("0, 300000;\r\n"));
        }

        [Test]
        public void CALLING_WRITE_TO_FILE_SHOULD_ADD_CALCULATED_ID_AND_TIME_TO_FILE()
        {
            int firstKeyCombId = HelperFunctions.GetCombinationId('a', '3');
            int secondKeyCombId = HelperFunctions.GetCombinationId('G', 'B');

            KeyCombination firstKeyComb = new KeyCombination(firstKeyCombId);
            KeyCombination secondKeyComb = new KeyCombination(secondKeyCombId);

            keyCombinations = new KeyCombination[2];
            firstKeyComb.AddTimespanToList(new TimeSpan(200000));
            secondKeyComb.AddTimespanToList(new TimeSpan(300000));

            keyCombinations[0] = firstKeyComb;
            keyCombinations[1] = secondKeyComb;

            ttfRecorder.WriteRecordedDataToFile(keyCombinations, @"D:\File\");

            string textContents = fileSystem.GetFile(@"D:\File\KeyboardData.txt").TextContents;

            Assert.That(textContents, Does.Contain($"{firstKeyCombId}, 200000;\r\n"));
            Assert.That(textContents, Does.Contain($"{secondKeyCombId}, 300000;\r\n"));
        }

        [Test]
        public void WRITTING_TO_FILE_SHOULD_CATCH_NULL_KEY_COMBINATION_OBJECTS()
        {
            keyCombinations = new KeyCombination[1];
            keyCombinations[0] = null;     
            ttfRecorder.WriteRecordedDataToFile(keyCombinations, @"D:\File\");
        }
    }
}
