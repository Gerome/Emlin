using NUnit.Framework;
using Emlin;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Threading;

namespace EmlinTests
{
    [TestFixture]
    public class EmlinKeyboardTests
    {
        char[] listOfInputs;
        List<char> listOfKeysEntered;
        KeyboardRecorder kbRec = new KeyboardRecorder();

        public EmlinKeyboardTests()
        {
            
            listOfInputs = kbRec.ListOfInputs;
            listOfKeysEntered = kbRec.KeysEntered;
        }

        [Test]
        public void KEYBOARD_RECORDER_SHOULD_CREATE_AN_ARRAY()
        {
            Assert.NotNull(listOfInputs, null);
        }

        [Test]
        public void KEYBOARD_RECORDER_INPUT_LIST_SHOULD_BE_202_IN_LENGTH()
        {
            int numberOfKeyInputs = 194; 
            Assert.That(numberOfKeyInputs, Is.EqualTo(listOfInputs.Length));
        }
        
        [Test]
        public void PRESSING_A_KEY_SHOULD_ADD_KEY_TO_ARRAYLIST_OF_PRESSED_KEYS()
        {
            kbRec.Keypressed(null, new KeyPressEventArgs('A'));         
            Assert.That(listOfKeysEntered.Contains('A'));
        }

        [Test]
        public void PRESSING_A_KEY_SHOULD_ADD_ANOTHER_KEY_TO_ARRAYLIST_OF_PRESSED_KEYS()
        {
            kbRec.Keypressed(null, new KeyPressEventArgs('B'));
            Assert.That(listOfKeysEntered.Contains('B'));
        }
    }
}
