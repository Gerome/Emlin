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
            ttfRecorder = new TimeToFileRecorder();
            ttfRecorder.AddFileSystem(fileSystem);
        }

        [TearDown]
        public void Dispose()
        {
            fileSystem.File.Delete(@"KeyboardData.txt");
        }

        [Test]
        public void CALLING_WRITE_TO_FILE_SHOULD_ADD_CURRENT_DATA_TO_FILE()
        {
            KeyCombination keyComb = new KeyCombination(0);

            keyCombinations = new KeyCombination[1];
            keyComb.AddTimespanToList(new TimeSpan(300000));

            keyCombinations[0] = keyComb;

            ttfRecorder.WriteRecordedDataToFile(keyCombinations, @"D:\File\");

            Assert.That(fileSystem.GetFile(@"D:\File\KeyboardData.txt").TextContents, Does.Contain("0, 300000\r\n"));
        }

        [Test]
        public void CALLING_WRITE_TO_FILE_SHOULD_ADD_CALCULATED_ID_AND_TIME_TO_FILE()
        {

            KeyCombination firstKeyComb = new KeyCombination(0);
            KeyCombination secondKeyComb = new KeyCombination(1);

            firstKeyComb.AddTimespanToList(new TimeSpan(200000));
            secondKeyComb.AddTimespanToList(new TimeSpan(300000));

            keyCombinations = new KeyCombination[2];
            keyCombinations[0] = firstKeyComb;
            keyCombinations[1] = secondKeyComb;

            ttfRecorder.WriteRecordedDataToFile(keyCombinations, @"D:\File\");

            string textContents = fileSystem.GetFile(@"D:\File\KeyboardData.txt").TextContents;

            Assert.That(textContents, Does.Contain("0, 200000\r\n"));
            Assert.That(textContents, Does.Contain("1, 300000\r\n"));
        }

        [Test]
        public void CALLING_WRITE_TO_FILE_WITH_SAME_COMBINATION_SHOULD_ADD_TIMES_TO_SAME_LINE()
        {

            KeyCombination keyComb = new KeyCombination(4);

            keyComb.AddTimespanToList(new TimeSpan(200000));
            keyComb.AddTimespanToList(new TimeSpan(300000));

            keyCombinations = new KeyCombination[1];
            keyCombinations[0] = keyComb;

            ttfRecorder.WriteRecordedDataToFile(keyCombinations, @"D:\File\");

            string textContents = fileSystem.GetFile(@"D:\File\KeyboardData.txt").TextContents;

            Assert.That(textContents, Does.Contain("4, 200000, 300000\r\n"));
        }

        [Test]
        public void WRITING_TO_FILE_SHOULD_CATCH_NULL_KEY_COMBINATION_OBJECTS()
        {
            keyCombinations = new KeyCombination[1];
            keyCombinations[0] = null;     
            ttfRecorder.WriteRecordedDataToFile(keyCombinations, @"D:\File\");
        }

        [Test]
        public void WRITING_TO_A_DIRECTORY_THAT_DOESNT_EXIST_SHOULD_CREATE_THE_DIRECTORY()
        {
            ttfRecorder.WriteRecordedDataToFile(keyCombinations, @"D:\FileDoesntExist\");
        }
    }
}
