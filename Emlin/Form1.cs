using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Emlin
{
    public partial class Form1 : Form
    {
        public KeyboardRecorder kbRec;

        public Form1()
        {
            InitializeComponent();
            kbRec = new KeyboardRecorder();
            this.KeyPress += new KeyPressEventHandler(kbRec.Keypressed);
        } 
    }
}
