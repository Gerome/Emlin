using Emlin;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;

namespace EmlinTests
{
    class DataToFileWriterTests
    {
        private List<KeysData> keysData;
        private MockFileSystem fileSystem;
        private DataToFileWriter ttfRecorder;
        private string textContents;
        

        [SetUp]
        public void SetUp()
        {
            keysData = new List<KeysData>();
            fileSystem = new MockFileSystem();
            ttfRecorder = new DataToFileWriter();
            ttfRecorder.AddFileSystem(fileSystem);
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
            ttfRecorder.WriteRecordedDataToFile(keysData, @"D:\Directory\File.txt");
            textContents = GetContentsOfTextFile();
        }

        [Test]
        public void Prepare_file_for_writing_should_create_the_directory_its_writing_to_if_it_doesnt_exist()
        {
            PrepareFileForWriting();
            Assert.That(fileSystem.Directory.Exists(@"D:\Directory"), Is.True);
        }

        [Test]
        public void Prepare_file_for_writing_should_not_create_the_directory_with_the_same_name_as_the_file()
        {
            PrepareFileForWriting();
            Assert.That(fileSystem.Directory.Exists(@"D:\Directory\File.txt"), Is.False);
        }

        [Test]
        public void Prepare_file_for_writing_should_create_the_file_its_writing_to_if_it_doesnt_exist()
        {
            PrepareFileForWriting();
            Assert.That(fileSystem.File.Exists(@"D:\Directory\File.txt"), Is.True);
        }

        [Test]
        public void Write_data_recorder_should_add_a_single_holdtime_to_the_text_file()
        {
            PrepareFileForWriting();
            keysData.Add(NewKeysData(0, 100));
            WriteDataToFile();

            Assert.That(textContents, Contains.Substring("0,100"));
        }

        [Test]
        public void Write_data_recorder_should_add_a_multiple_holdtime_to_the_text_file()
        {
            PrepareFileForWriting();
            keysData.Add(NewKeysData(0, 100));
            keysData.Add(NewKeysData(1, 200));
            WriteDataToFile();

            Assert.That(textContents, Contains.Substring("0,100"));
            Assert.That(textContents, Contains.Substring("1,200"));
        }

        [Test]
        public void Write_data_recorder_should_add_a_single_flight_time_to_the_text_file()
        {
            PrepareFileForWriting();
            keysData.Add(NewKeysData(0, 0, 100));
            WriteDataToFile();

            Assert.That(textContents, Contains.Substring("0,0,100"));
        }

        [Test]
        public void Write_data_recorder_should_add_multiple_flight_times_to_the_text_file()
        {
            PrepareFileForWriting();
            keysData.Add(NewKeysData(0, 0, 100));
            keysData.Add(NewKeysData(1, 0, 260));
            WriteDataToFile();

            Assert.That(textContents, Contains.Substring("0,0,100"));
            Assert.That(textContents, Contains.Substring("1,0,260"));
        }

        [Test]
        public void Write_data_recorder_should_add_a_single_digraph1_time_to_the_text_file()
        {
            PrepareFileForWriting();
            keysData.Add(NewKeysData(0, 0, 100, 200));
            WriteDataToFile();

            Assert.That(textContents, Contains.Substring("0,0,100,200"));
        }

        [Test]
        public void Write_data_recorder_should_add_multiple_digraph1_times_to_the_text_file()
        {
            PrepareFileForWriting();
            keysData.Add(NewKeysData(0, 0, 100, 200));
            keysData.Add(NewKeysData(1, 666, 666, 42));
            WriteDataToFile();

            Assert.That(textContents, Contains.Substring("0,0,100,200"));
            Assert.That(textContents, Contains.Substring("1,666,666,42"));
        }

        [Test]
        public void Write_data_recorder_should_add_a_single_digraph2_time_to_the_text_file()
        {
            PrepareFileForWriting();
            keysData.Add(NewKeysData(0, 0, 100, 200, 300));
            WriteDataToFile();

            Assert.That(textContents, Contains.Substring("0,0,100,200,300"));
        }


        [Test]
        public void Write_data_recorder_should_add_multiple_digraph2_times_to_the_text_file()
        {
            PrepareFileForWriting();
            keysData.Add(NewKeysData(0, 0, 100, 200, 300));
            keysData.Add(NewKeysData(1, 666, 666, 42, 96));
            WriteDataToFile();

            Assert.That(textContents, Contains.Substring("0,0,100,200,300"));
            Assert.That(textContents, Contains.Substring("1,666,666,42,96"));
        }

        private KeysData NewKeysData(int combID, int Ht, int Ft, int D1, int D2)
        {
            return new KeysData
            {
                CombinationID = combID,
                HoldTime = TimeSpan.FromMilliseconds(Ht),
                FlightTime = TimeSpan.FromMilliseconds(Ft),
                Digraph1 = TimeSpan.FromMilliseconds(D1),
                Digraph2 = TimeSpan.FromMilliseconds(D2)
            };
        }

        private KeysData NewKeysData(int combID, int Ht, int Ft, int D1)
        {
            return NewKeysData(combID, Ht, Ft, D1, 0);
        }

        private KeysData NewKeysData(int combID, int Ht, int Ft)
        {
            
            return NewKeysData(combID, Ht, Ft, 0);
        }

        private KeysData NewKeysData(int combID, int Ht)
        {
            return NewKeysData(combID,Ht,0);
        }
    }
}
