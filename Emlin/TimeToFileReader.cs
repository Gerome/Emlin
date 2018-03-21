using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;

namespace Emlin
{


    public class TimeToFileReader
    {

        IFileSystem fileSystem;

        public TimeToFileReader()
        {
            this.fileSystem = new FileSystem();
        }

        public void AddFileSystem(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public List<KeyCombination> ReadDataToObject(string filepath)
        {
            List<KeyCombination> keyCombinations = new List<KeyCombination>();

            StreamReader streamReader = fileSystem.File.OpenText(filepath);
            
            CsvReader csvReader = new CsvReader(streamReader);

            while (csvReader.Read())
            {
                string combId = csvReader[0];

                KeyCombination keyComb = new KeyCombination(int.Parse(combId));


                for (int i = 1; csvReader.TryGetField<string>(i, out string value); i++)
                {
                    long timeInTickLong = long.Parse(value);
                    TimeSpan timeInTicks = new TimeSpan(timeInTickLong);
                    keyComb.AddTimespanToList(timeInTicks);
                }

                keyCombinations.Add(keyComb);
            }


            return keyCombinations;
        }
    }
}
