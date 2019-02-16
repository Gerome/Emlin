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
            Decryptor.DecryptFiles();
        }

        private void TeachModelBtn_Click(object sender, EventArgs e)
        {
            HealthSubject hs = new HealthSubject();
            hs.SetValue(100);
            pi.TeachModel(comboBoxModelSelection.Text, hs);
            textBox1.AppendText("Started running python scripts." + Environment.NewLine);
        }

        private void LoadModel_Click(object sender, EventArgs e)
        {
            HealthSubject hs = new HealthSubject();
            hs.SetValue(100);
            pi.TestUserInput(new List<string> { "13413,83,51,135,160,244", "12222,586,-243,343,-103,483" }, hs);
        }

        private void GenerateInverseUserData_Click(object sender, EventArgs e)
        {
            pi.GenerateNonUserData();
        }
    }
}
