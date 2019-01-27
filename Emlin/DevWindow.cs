using Emlin.Encryption;
using Emlin.Python;
using System;
using System.Windows.Forms;

namespace Emlin
{
    public partial class DevWindow : Form
    {
        public DevWindow()
        {
            InitializeComponent();
        }

        private void decryptBtn_Click(object sender, EventArgs e)
        {
            Decryptor.DecryptFiles();
        }

        private void TeachModelBtn_Click(object sender, EventArgs e)
        {
            PythonInterface pi = new PythonInterface();
            pi.TeachModel();

            textBox1.AppendText("Started running python scripts." + Environment.NewLine);
        }

        private void LoadModel_Click(object sender, EventArgs e)
        {
            PythonInterface pi = new PythonInterface();
            pi.TestUserInput("1186,118,791,910,1028,1147");
        }
    }
}
