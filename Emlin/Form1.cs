using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Emlin
{
    public partial class Form1 : Form
    {
        public KeyboardRecorder kbRec;
        CurrentSession currentSession;

        public Form1()
        {
            InitializeComponent();
            CustomTimer timer = new CustomTimer(ConstantValues.LENGTH_OF_SESSION_IN_MILLIS);
            timer.Tick += TimerCountdown;

            this.KeyPress += new KeyPressEventHandler(Keypressed);

            currentSession = new CurrentSession(timer);
        }

        #region methods

        void Keypressed(Object o, KeyPressEventArgs e)
        {
            SendKeyToCurrentSession(e.KeyChar, DateTime.Now.Ticks);
        }

        void TimerCountdown(object sender, EventArgs e)
        {
            string filepath = ConstantValues.KEYBOARD_DATA_FILEPATH + @"\KeyboardData.txt";

            TimeToFileReader ttfReader = new TimeToFileReader();
            List<KeyCombination> keyCombinationsInFile = ttfReader.ReadDataToObject(filepath);

            List<KeyCombination> keyCombinationsToWrite = MergeList.MergeKeyboardCombinationList(keyCombinationsInFile, currentSession.KeysPressed);

            File.WriteAllText(filepath, String.Empty);

            TimeToFileRecorder ttfRec = new TimeToFileRecorder();
            ttfRec.WriteRecordedDataToFile(keyCombinationsToWrite, filepath);
            currentSession.End();
        }

        private void SendKeyToCurrentSession(char keyChar, long ticks)
        {
            currentSession.KeyWasPressed(keyChar, ticks);
        }

        #endregion
    }
}
