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

        private void SendKeyToCurrentSession(char keyChar, long ticks)
        {
            currentSession.KeyWasPressed(keyChar, ticks);
        }

        void Keypressed(Object o, KeyPressEventArgs e)
        {
            SendKeyToCurrentSession(e.KeyChar, DateTime.Now.Ticks);
        }

        void TimerCountdown(object sender, ElapsedEventArgs e)
        {
            TimeToFileRecorder ttfRec = new TimeToFileRecorder();
            ttfRec.WriteRecordedDataToFile(currentSession.KeysPressed, ConstantValues.KEYBOARD_DATA_FILEPATH);
            currentSession.End();
        }

        #endregion
    }
}
