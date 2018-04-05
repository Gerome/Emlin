using Emlin;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;

namespace EmlinTests
{
    class TimeToFileRecorderTests
    {
        private List<KeyCombination> keyCombinations = new List<KeyCombination>();
        private MockFileSystem fileSystem = new MockFileSystem();
        private TimeToFileRecorder ttfRecorder;

        [SetUp]
        public void Init()
        {
            ttfRecorder = new TimeToFileRecorder();
            fileSystem.AddDirectory(@"D:\File");
            ttfRecorder.AddFileSystem(fileSystem);
            SetUpTestData();
        }

        KeyCombination keyComb;
        KeyCombination secondKeyComb;
        string textContents;

        private void SetUpTestData()
        {
            keyComb = new KeyCombination(0);
            keyComb.AddTimespanToList(new TimeSpan(200000));
            keyComb.AddTimespanToList(new TimeSpan(999999));

            secondKeyComb = new KeyCombination(1);
            secondKeyComb.AddTimespanToList(new TimeSpan(300000));

            keyCombinations.Add(keyComb);
            keyCombinations.Add(secondKeyComb);

            ttfRecorder.WriteRecordedDataToFile(keyCombinations, @"D:\File\KeyboardData.txt");

            textContents = fileSystem.GetFile(@"D:\File\KeyboardData.txt").TextContents;
        }

        [Test]
        public void CALLING_WRITE_TO_FILE_SHOULD_ADD_CURRENT_DATA_TO_FILE()
        {
           
            Assert.That(textContents, Does.Contain("0, 200000"));
        }

        [Test]
        public void CALLING_WRITE_TO_FILE_SHOULD_ADD_CALCULATED_ID_AND_TIME_TO_FILE()
         {
          
            Assert.That(textContents, Does.Contain("1, 300000\r\n"));
        }

        [Test]
        public void CALLING_WRITE_TO_FILE_WITH_SAME_COMBINATION_SHOULD_ADD_TIMES_TO_SAME_LINE()
        {
           
            Assert.That(textContents, Does.Contain("0, 200000, 999999\r\n"));
        }

        [Test]
        public void WRITING_TO_FILE_SHOULD_CATCH_NULL_KEY_COMBINATION_OBJECTS()
        {
            keyCombinations.Add(null);     
            ttfRecorder.WriteRecordedDataToFile(keyCombinations, @"D:\File\KeyboardData.txt");
        }

        [Test]
        public void WRITING_TO_A_DIRECTORY_THAT_DOESNT_EXIST_SHOULD_CREATE_THE_DIRECTORY()
        {
            ttfRecorder.WriteRecordedDataToFile(keyCombinations, @"D:\FileDoesntExist");
            Assert.That(fileSystem.FileExists(@"D:\FileDoesntExist"), Is.True);
        }
    }
}
