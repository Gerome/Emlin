using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Forms;

namespace Emlin
{
    
    public static class PythonPathGetter
    {
        public static string GetPythonExePath()
        {
            string pythonPath = "";

            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Python\PythonCore\3.7\InstallPath"))
                {

                    object o = key.GetValue(null);

                    pythonPath = o.ToString();
                }


            }
            catch (NullReferenceException)
            {
                string warning = "You don't have Python installed on this machine.";
                string title = "Error";
                MessageBox.Show(warning, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return pythonPath;
            }

            return Path.Combine(pythonPath, @"python.exe");
        }
    }
    
}
