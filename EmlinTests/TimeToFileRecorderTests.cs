using Emlin;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;

namespace EmlinTests
{
    class TimeToFileRecorderTests
    {
        private List<KeyCombination> keyCombinations;
        private MockFileSystem fileSystem;
        private TimeToFileRecorder ttfRecorder;
        private string textContents;

        string newLine = "\r\n";

        [SetUp]
        public void SetUp()
        {
            keyCombinations = new List<KeyCombination>();
            fileSystem = new MockFileSystem();
            ttfRecorder = new TimeToFileRecorder();
            ttfRecorder.AddFileSystem(fileSystem);
        }

        [TearDown]
        public void TearDown()
        {

        }

        private void PrepareFileForWriting()
        {
            ttfRecorder.CreateDirectoryAndFile(@"D:\Directory\File.txt");
            textContents = GetContentsOfTextFile();
        }

        private string GetContentsOfTextFile()
        {
            return fileSystem.GetFile(@"D:\Directory\File.txt").TextContents;
        }

        private void WriteDataToFile()
        {
            ttfRecorder.WriteRecordedDataToFile(keyCombinations, @"D:\Directory\File.txt");
            textContents = GetContentsOfTextFile();
        }

        [Test]
        public void PREPARE_FILE_FOR_WRITING_SHOULD_CREATE_THE_DIRECTORY_ITS_WRITING_TO_IF_IT_DOESNT_EXIST()
        {
            PrepareFileForWriting();
            Assert.That(fileSystem.Directory.Exists(@"D:\Directory"), Is.True);
        }

        [Test]
        public void PREPARE_FILE_FOR_WRITING_SHOULD_NOT_CREATE_THE_DIRECTORY_WITH_THE_SAME_NAME_AS_THE_FILE()
        {
            PrepareFileForWriting();
            Assert.That(fileSystem.Directory.Exists(@"D:\Directory\File.txt"), Is.False);
        }

        [Test]
        public void PREPARE_FILE_FOR_WRITING_SHOULD_CREATE_THE_FILE_ITS_WRITING_TO_IF_IT_DOESNT_EXIST()
        {
            PrepareFileForWriting();
            Assert.That(fileSystem.File.Exists(@"D:\Directory\File.txt"), Is.True);
        }


        [Test]
        public void PREPARE_FILE_FOR_WRITING_SHOULD_POPULATE_THE_TEXT_FILE_WITH_INDEX_FROM_ZERO_TO_TOTAL_NUMBER_OF_COMBINATIONS_IF_THE_FILE_IS_EMPTY()
        {
            fileSystem.File.Create(@"D:\Directory\File.txt");

            ttfRecorder.PopulateTextFileIfEmpty(@"D:\Directory\File.txt");
            textContents = GetContentsOfTextFile();
            AssertTextFileContainsIndexNumbers();
        }

        [Test]
        public void WRITE_DATA_RECORDER_SHOULD_ADD_A_SINGLE_TIMESPAN_TO_THE_TEXT_FILE()
        {
            KeyCombination keyComb = new KeyCombination(0);
            keyComb.AddTimespanToList(new TimeSpan(20000000));
            keyCombinations.Add(keyComb);

            fileSystem.AddFile(@"D:\Directory\File.txt", "0");

            WriteDataToFile();

            Assert.That(textContents, Contains.Substring("0, 2000"));
        }

        [Test]
        public void WRITE_DATA_RECORDER_SHOULD_ROUND_A_TIMESPAN_TO_THE_NEAREST_MILLISECOND()
        {
            KeyCombination keyComb = new KeyCombination(0);
            keyComb.AddTimespanToList(new TimeSpan(12345679));
            keyCombinations.Add(keyComb);

            fileSystem.AddFile(@"D:\Directory\File.txt", "0");

            WriteDataToFile();

            Assert.That(textContents, Contains.Substring("0, 1234.5679" + newLine));
        }

        [Test]
        public void WRITE_DATA_RECORDER_SHOULD_ADD_A_MULTIPLE_TIMESPANS_TO_THE_TEXT_FILE()
        {
            KeyCombination keyComb = new KeyCombination(0);
            keyComb.AddTimespanToList(new TimeSpan(20000000));
            keyComb.AddTimespanToList(new TimeSpan(3000000));
            keyCombinations.Add(keyComb);

            fileSystem.AddFile(@"D:\Directory\File.txt", "0");

            WriteDataToFile();

            Assert.That(textContents, Contains.Substring("0, 2000, 300"));
        }

        [Test]
        public void WRITE_DATA_RECORDER_SHOULD_ADD_A_MULTIPLE_KEYCOMBINATIONS_IN_SEQUENCE_TO_THE_TEXT_FILE()
        {
            KeyCombination keyComb1 = new KeyCombination(0);
            KeyCombination keyComb2 = new KeyCombination(1);
            keyComb1.AddTimespanToList(new TimeSpan(20000000));
            keyComb2.AddTimespanToList(new TimeSpan(20000000));
            keyCombinations.Add(keyComb1);
            keyCombinations.Add(keyComb2);

            string indexValues = 
                "0" + newLine +
                "1";

            fileSystem.AddFile(@"D:\Directory\File.txt", indexValues);

            WriteDataToFile();

            string actual =
                "0, 2000" + newLine +
                "1, 2000";

            Assert.That(textContents, Contains.Substring(actual));
        }

        [Test]
        public void WRITE_DATA_RECORDER_SHOULD_ADD_A_MULTIPLE_KEYCOMBINATIONS_NOT_IN_SEQUENCE_TO_THE_TEXT_FILE()
        {
            KeyCombination keyComb1 = new KeyCombination(0);
            KeyCombination keyComb2 = new KeyCombination(2);
            keyComb1.AddTimespanToList(new TimeSpan(20000000));
            keyComb2.AddTimespanToList(new TimeSpan(40000000));
            keyCombinations.Add(keyComb1);
            keyCombinations.Add(keyComb2);

            string indexValues = 
                "0" + newLine +
                "1" + newLine +
                "2";

            fileSystem.AddFile(@"D:\Directory\File.txt", indexValues);

            WriteDataToFile();

            string actual =
                "0, 2000" + newLine +
                "1"       + newLine +
                "2, 4000";

            Assert.That(textContents, Contains.Substring(actual));
        }

        private void AssertTextFileContainsIndexNumbers()
        {
            string zero = "0";
            string one = "1";
            string random = "200";
            string lastIndex = (ConstantValues.NUMBER_OF_COOMBINATIONS - 1).ToString();

            StringAssert.Contains(zero, textContents);
            StringAssert.Contains(one, textContents);
            StringAssert.Contains(random, textContents);
            StringAssert.Contains(lastIndex, textContents);
        }
    }
}
