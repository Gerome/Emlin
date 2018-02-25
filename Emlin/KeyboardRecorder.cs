using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emlin
{
    public class KeyboardRecorder
    {
        private char[] listOfInputs;

        public KeyboardRecorder()
        {
            ListOfInputs = new char[104];
        }

        public char[] ListOfInputs { get => listOfInputs; set => listOfInputs = value; }
    }
}
