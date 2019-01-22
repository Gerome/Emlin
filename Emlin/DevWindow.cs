using Emlin.Encryption;
using Emlin.Python;
using System;
using System.IO;
using System.Text.RegularExpressions;
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
            DecryptFiles();
        }

        private static void DecryptFiles()
        {

            DirectoryInfo d = new DirectoryInfo(Environment.CurrentDirectory + @"\..\..\..\..\Data\Raw");
            FileInfo[] Files = d.GetFiles("KeyboardData_*.txt"); //Getting Text files
            foreach (FileInfo file in Files)
            {
                DecryptFile(file);
            }
        }


        private static void DecryptFile(FileInfo file)
        {
            IEncryptor decryptor = new Encryptor();

            var lines = File.ReadLines(file.FullName);
            WriteDecryptedLinesToFile(file.Name, decryptor, lines);
        }

        private static void WriteDecryptedLinesToFile(string fileName, IEncryptor decryptor, System.Collections.Generic.IEnumerable<string> lines)
        {
            using (StreamWriter sw = File.CreateText(Environment.CurrentDirectory + @"\..\..\..\..\Data\Interim\D_" + fileName))
            {
                string fileNumber = GetFileNumber(fileName);

                foreach (string line in lines)
                {
                    sw.Write(decryptor.Decrypted(line, fileNumber));
                }
            }
        }

        private static string GetFileNumber(string fileName)
        {
            return Regex.Match(fileName, @"\d+").Value;
        }

        private void RunPython_Click(object sender, EventArgs e)
        {
            PythonInterface.TestRunPython();
            textBox1.AppendText("Started running python scripts." + Environment.NewLine);
        }
    }
}
