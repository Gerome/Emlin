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
        Dictionary<int, List<char>> dictOfCombinations;
        KeyboardRecorder kbRec = new KeyboardRecorder();


        //private int DECIMAL_ASCII_OF_FIRST_CHAR = 32;
        //private int DECIMAL_ASCII_OF_LAST_CHAR  = 128;

        
        [SetUp]
        public void Init()
        {    
            listOfInputs = kbRec.ListOfInputs;
            dictOfCombinations = kbRec.DictOfCombinations;
        }

        

        [Test]
        public void KEYBOARD_RECORDER_SHOULD_CREATE_AN_ARRAY_OF_LIST_OF_INPUTS()
        {
            Assert.NotNull(listOfInputs);
        }

        [Test]
        public void LIST_OF_INPUTS_SHOULD_BE_96_CHARACTERS_IN_LENGTH()
        {
            Assert.That(listOfInputs.Length, Is.EqualTo(ConstantValues.NUMBER_OF_INPUTS));
        }

        [Test]
        public void FIRST_CHARACTER_IN_LIST_OF_INPUTS_SHOULD_BE_THE_SPACE_CHARACTER()
        {
            Assert.That(listOfInputs[0], Is.EqualTo((char)Keys.Space));
        }

        [Test]
        public void LAST_CHARACTER_IN_LIST_OF_INPUTS_SHOULD_BE_THE_DELETE_CHARACTER()
        {
            Assert.That(listOfInputs[ConstantValues.NUMBER_OF_INPUTS - 1], Is.EqualTo('\u007f'));
        }

        [Test]
        public void KEYBOARD_RECORDER_SHOULD_CREATE_AN_ARRAY_OF_LIST_OF_COMBINATIONS()
        {
            Assert.NotNull(dictOfCombinations);
        }

        [Test]
        public void LIST_OF_COMBINATIONS_SHOULD_BE_9216_IN_LENGTH()
        {
            Assert.That(dictOfCombinations.Count, Is.EqualTo(ConstantValues.NUMBER_OF_COOMBINATIONS));
        }

        [Test]
        public void FIRST_VALUE_IN_COMBINATIONS_SHOULD_SPACE_SPACE()
        {  
            Assert.That(dictOfCombinations[0], Is.EqualTo(new List<char>() { ' ', ' ' }));
        }

        [Test]
        public void LAST_VALUE_IN_COMBINATIONS_SHOULD_SPACE_SPACE()
        {
            Assert.That(dictOfCombinations[ConstantValues.NUMBER_OF_COOMBINATIONS - 1], Is.EqualTo(new List<char>() { '\u007f', '\u007f' }));
        }

        [Test]
        public void PRESSING_TWO_KEYS_CREATES_COMBINATION_OBJECT()
        {
            int combId = HelperFunctions.GetCombinationId('A', 'B');
            KeyCombination keyComb = new KeyCombination(combId);
            keyComb.AddTimespanToList(new TimeSpan(300000));

            Assert.That(keyComb.TimeSpanList[0].Ticks, Is.EqualTo(300000));
        }


        



        /*
        * --TEST LIST--
        */
    }
}
