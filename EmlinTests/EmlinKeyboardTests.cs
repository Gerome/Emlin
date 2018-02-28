﻿using NUnit.Framework;
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
        //List<char> listOfKeysEntered;
        KeyboardRecorder kbRec = new KeyboardRecorder();


        //private int DECIMAL_ASCII_OF_FIRST_CHAR = 32;
        //private int DECIMAL_ASCII_OF_LAST_CHAR  = 128;
        private int NUMBER_OF_INPUTS            = 96;
        private int NUMBER_OF_COOMBINATIONS     = 96 * 96;

        public EmlinKeyboardTests()
        {
            
            listOfInputs = kbRec.ListOfInputs;
            dictOfCombinations = kbRec.DictOfCombinations;
            //listOfKeysEntered = kbRec.KeysEntered;
            
        }

        [Test]
        public void KEYBOARD_RECORDER_SHOULD_CREATE_AN_ARRAY_OF_LIST_OF_INPUTS()
        {
            Assert.NotNull(listOfInputs);
        }

        [Test]
        public void LIST_OF_INPUTS_SHOULD_BE_96_CHARACTERS_IN_LENGTH()
        {
            Assert.That(listOfInputs.Length, Is.EqualTo(NUMBER_OF_INPUTS));
        }

        [Test]
        public void FIRST_CHARACTER_IN_LIST_OF_INPUTS_SHOULD_BE_THE_SPACE_CHARACTER()
        {
            Assert.That(listOfInputs[0], Is.EqualTo((char)Keys.Space));
        }

        [Test]
        public void LAST_CHARACTER_IN_LIST_OF_INPUTS_SHOULD_BE_THE_DELETE_CHARACTER()
        {
            Assert.That(listOfInputs[NUMBER_OF_INPUTS - 1], Is.EqualTo('\u007f'));
        }

        [Test]
        public void KEYBOARD_RECORDER_SHOULD_CREATE_AN_ARRAY_OF_LIST_OF_COMBINATIONS()
        {
            Assert.NotNull(dictOfCombinations);
        }

        [Test]
        public void LIST_OF_COMBINATIONS_SHOULD_BE_9216_IN_LENGTH()
        {
            Assert.That(dictOfCombinations.Count, Is.EqualTo(NUMBER_OF_COOMBINATIONS));
        }

        [Test]
        public void FIRST_VALUE_IN_COMBINATIONS_SHOULD_SPACE_SPACE()
        {
            
            Assert.That(dictOfCombinations[0], Is.EqualTo(new List<char>() { ' ', ' ' }));
        }

        [Test]
        public void LAST_VALUE_IN_COMBINATIONS_SHOULD_SPACE_SPACE()
        {

            Assert.That(dictOfCombinations[NUMBER_OF_COOMBINATIONS - 1], Is.EqualTo(new List<char>() { '\u007f', '\u007f' }));
        }

        //[Test]
        //public void PRESSING_A_KEY_SHOULD_ADD_KEY_TO_ARRAYLIST_OF_PRESSED_KEYS()
        //{
        //    //kbRec.Keypressed(null, new KeyPressEventArgs('A'));         
        //    //Assert.That(listOfKeysEntered.Contains('A'));
        //}

        //[Test]
        //public void PRESSING_A_KEY_SHOULD_ADD_ANOTHER_KEY_TO_ARRAYLIST_OF_PRESSED_KEYS()
        //{
        //    //kbRec.Keypressed(null, new KeyPressEventArgs('B'));
        //    //Assert.That(listOfKeysEntered.Contains('B'));
        //}

        //[Test]
        //public void PRESSING_TWO_KEYS_AT_DIFFERENT_TIMES_SHOULD_RECORD_THE_DIFFERENCE()
        //{
        //    //kbRec.Keypressed(null, new KeyPressEventArgs('A'));
        //    //Thread.Sleep(new TimeSpan(300000));
        //    //kbRec.Keypressed(null, new KeyPressEventArgs('B'));
        //    //Assert.That(keysTyped)
        //}

        /*
            * --TEST LIST--
            * List of combinations should contain all possible combinations
            * 
            * 
            * PRESSING TWO KEYS IN SUCCESSION SHOULD RECORD BOTH KEYS AND THE TICKS BETWEEN THEM
            * TWO KEYS PRESSED IN LESS THAN 1.5(?) SECONDS SHOULD NOT RECORD THE DIFFERENCE
            */
    }
}
