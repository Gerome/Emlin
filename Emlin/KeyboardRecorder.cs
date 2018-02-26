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
        private char[] listOfInputs;
        private int numberOfInputs = Enum.GetNames(typeof(Keys)).Length;

        private List<char> keysEntered = new List<char>();

        public KeyboardRecorder()
        {
            ListOfInputs = new char[numberOfInputs];
        }

        public char[] ListOfInputs { get => listOfInputs; set => listOfInputs = value; }
        public List<char> KeysEntered { get => keysEntered; set => keysEntered = value; }

        public void Keypressed(Object o, KeyPressEventArgs e)
        {
            KeysEntered.Add(e.KeyChar);         
        }
    }
}
