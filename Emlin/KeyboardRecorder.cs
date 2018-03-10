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
        private const int DECIMAL_ASCII_OF_FIRST_CHAR = 32;
 

        private char[] listOfInputs;
        private Dictionary<int, List<char>> dictOfCombinations;
        private List<char> keysEntered = new List<char>();

        public char[] ListOfInputs { get => listOfInputs; set => listOfInputs = value; }
        public Dictionary<int, List<char>> DictOfCombinations { get => dictOfCombinations; set => dictOfCombinations = value; }

        public CurrentSession currentSession = new CurrentSession();
        
        public KeyboardRecorder()
        {

            listOfInputs = new char[ConstantValues.NUMBER_OF_INPUTS];
            dictOfCombinations = new Dictionary<int, List<char>>();
            PopulateListOfInputs();
            PopulateListOfCombinations();

        }

        private void PopulateListOfInputs()
        {
            for (int i = 0; i < ConstantValues.NUMBER_OF_INPUTS; i++)
            {
                listOfInputs[i] = (char)(i + DECIMAL_ASCII_OF_FIRST_CHAR);
            }
        }

        private void PopulateListOfCombinations()
        {
            int index;
            for (int i = 0; i < ConstantValues.NUMBER_OF_INPUTS; i++)
            {
                for (int n = 0; n < ConstantValues.NUMBER_OF_INPUTS; n++)
                {
                    index = i * ConstantValues.NUMBER_OF_INPUTS + n;
                    dictOfCombinations.Add(index, new List<char>() { ListOfInputs[i], ListOfInputs[n] });
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
