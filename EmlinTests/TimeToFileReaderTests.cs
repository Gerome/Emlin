using Emlin;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;

namespace EmlinTests
{
    class TimeToFileReaderTests
    {
        TimeToFileReader ttfReader;
        private string testData = @"
4
12345, 30000
";

        private MockFileSystem fileSystem = new MockFileSystem();
        List<KeyCombination> keyCombActualList = new List<KeyCombination>();
        KeyCombination keyCombExpected;

        [SetUp]
        public void Init()
        {
            fileSystem.AddFile(@"C:\file", testData);
            ttfReader = new TimeToFileReader();
            ttfReader.AddFileSystem(fileSystem);

            keyCombActualList = ttfReader.ReadDataToObject(@"C:\file");        
        }

        [Test]
        public void READER_SHOULD_CREATE_KEY_COMBINATION_OBJECT_FROM_FILE_AND_STORE_THE_COMBINATION_ID()
        {
            keyCombExpected = new KeyCombination(4);
            Assert.That(keyCombActualList[0].CombId, Is.EqualTo(keyCombExpected.CombId));
        }


        [Test]
        public void READER_SHOULD_CREATE_ANOTHER_KEY_COMBINATION_OBJECT_FROM_FILE_AND_STORE_THE_COMBINATION_ID()
        {
            keyCombExpected = new KeyCombination(12345);
            Assert.That(keyCombActualList[1].CombId, Is.EqualTo(keyCombExpected.CombId));
        }

        [Test]
        public void READER_SHOULD_CREATE_KEY_COMBINATION_OBJECT_FROM_FILE_AND_STORE_THE_TIMESPAN()
        {
            keyCombExpected.AddTimespanToList(new TimeSpan(30000));
            Assert.That(keyCombActualList[1].TimeSpanList[0], Is.EqualTo(keyCombExpected.TimeSpanList[0]));
        }
    }
}
