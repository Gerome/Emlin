using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Emlin
{
    public class KeyboardRecorder
    {
        private int DECIMAL_ASCII_OF_FIRST_CHAR = 32;

        private int numberOfInputs;
        private int numberOfCombinations;

        private char[] listOfInputs;
        private Dictionary<int, List<char>> dictOfCombinations;
        private List<char> keysEntered = new List<char>();

        public char[] ListOfInputs { get => listOfInputs; set => listOfInputs = value; }
        public Dictionary<int, List<char>> DictOfCombinations { get => dictOfCombinations; set => dictOfCombinations = value; }


        //public List<char> KeysEntered { get => keysEntered; set => keysEntered = value; }

        //List<long> ticks = new List<long>();
        //List<long> timeDif = new List<long>();
        //List<long> timeDifInMillis = new List<long>();
        //Tuple<char, long> dict
        public KeyboardRecorder()
        {
            numberOfInputs = 128 - 32;
            numberOfCombinations = numberOfInputs * numberOfInputs;
            // Number of combinations is the number of inputs squared
            //numberOfCombinations = numberOfInputs * numberOfInputs;

            ListOfInputs = new char[numberOfInputs];
            DictOfCombinations = new Dictionary<int, List<char>>();
            PopulateListOfInputs();
            PopulateListOfCombinations();

        }

        private void PopulateListOfCombinations()
        {
            int index;
            for (int i = 0; i < numberOfInputs; i++)
            {
                for (int n = 0; n < numberOfInputs; n++)
                {
                    index = i * numberOfInputs + n;
                    DictOfCombinations.Add(index, new List<char>() { ListOfInputs[i], ListOfInputs[n] });
                }
            }
        }

        private void PopulateListOfInputs()
        {
            for (int i = 0; i < numberOfInputs; i++)
            {
                listOfInputs[i] = (char)(i + DECIMAL_ASCII_OF_FIRST_CHAR);
            }
        }

        #region public methods

        public void Keypressed(Object o, KeyPressEventArgs e)
        {
            //ticks.Add(DateTime.Now.Ticks);

            //if (timeDif.Count == 0)
            //{
            //    timeDif.Add(ticks[0]);
            //}
            //else
            //{
            //    timeDif.Add(ticks[ticks.Count - 1] - ticks[ticks.Count - 2]);
            //}
            //timeDifInMillis.Add(timeDif[timeDif.Count - 1] / 10000);
            //dict.Add(e.KeyChar, timeDifInMillis[timeDifInMillis.Count - 1]);
            //KeysEntered.Add(e.KeyChar);             
        }


        #endregion
    }
}
