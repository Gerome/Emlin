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

        public CurrentSession currentSession = new CurrentSession();

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

        private void PopulateListOfInputs()
        {
            for (int i = 0; i < numberOfInputs; i++)
            {
                listOfInputs[i] = (char)(i + DECIMAL_ASCII_OF_FIRST_CHAR);
            }
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

        #region public methods

        public void Keypressed(Object o, KeyPressEventArgs e)
        {
            SendKeyToCurrentSession(e.KeyChar, DateTime.Now.Ticks);  
        }

        public void SendKeyToCurrentSession(char keyChar, long ticks)
        {
            currentSession.KeyWasPressed(keyChar, ticks);
        }

        #endregion
    }
}
