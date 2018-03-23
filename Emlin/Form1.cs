using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

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
            timer.Elapsed += TimerCountdown;

            this.KeyPress += new KeyPressEventHandler(Keypressed);

            currentSession = new CurrentSession(timer);
        }

        #region methods

        void Keypressed(Object o, KeyPressEventArgs e)
        {
            SendKeyToCurrentSession(e.KeyChar, DateTime.Now.Ticks);
        }

        void TimerCountdown(object sender, ElapsedEventArgs e)
        {
            TimeToFileReader ttfReader = new TimeToFileReader();
            List<KeyCombination> keyCombinationsInFile = ttfReader.ReadDataToObject(ConstantValues.KEYBOARD_DATA_FILEPATH + @"\KeyboardData.txt");

            List<KeyCombination> keyCombinationsToWrite = MergeList.MergeKeyboardCombinationList(keyCombinationsInFile, currentSession.KeysPressed);

            TimeToFileRecorder ttfRec = new TimeToFileRecorder();
            ttfRec.WriteRecordedDataToFile(keyCombinationsToWrite, ConstantValues.KEYBOARD_DATA_FILEPATH + @"\KeyboardData.txt");
            currentSession.End();
        }

        private void SendKeyToCurrentSession(char keyChar, long ticks)
        {
            currentSession.KeyWasPressed(keyChar, ticks);
        }

        #endregion
    }
}
