using Emlin.Encryption;
using Emlin.Python;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Emlin
{
    public partial class DevWindow : Form
    {
        PythonInterface pi = new PythonInterface();

        public DevWindow()
        {
            InitializeComponent();
            comboBoxModelSelection.Text = comboBoxModelSelection.Items[0].ToString();
        }

        private void decryptBtn_Click(object sender, EventArgs e)
        {
            Decryptor.DecryptDevFiles();
        }

        private void TeachModelBtn_Click(object sender, EventArgs e)
        {
            HealthSubject hs = new HealthSubject();
            hs.SetValue(100);
            pi.TeachModel(comboBoxModelSelection.Text, ConstantValues.KEYBOARD_DATA_FILEPATH);
            textBox1.AppendText("Started running python scripts." + Environment.NewLine);
        }
    }
}
