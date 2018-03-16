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
            CustomTimer timer = (CustomTimer)new System.Timers.Timer();

            currentSession = new CurrentSession(timer);
            this.KeyPress += new KeyPressEventHandler(Keypressed);
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
