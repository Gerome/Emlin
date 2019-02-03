using Microsoft.Win32;
using System;

namespace Emlin
{
    public static class PythonPathGetter
    {
        public static string GetPythonDirectoryPath()
        {
            string pythonPath = "";

            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Python\PythonCore\3.7\InstallPath"))
                {
                    if (key != null)
                    {
                        object o = key.GetValue(null);

                        if (o != null)
                        {
                            pythonPath = o.ToString();
                        }
                    }
                }
            }
            catch (NullReferenceException ex)  //just for demonstration...it's always best to handle specific exceptions
            {
                //react appropriately
            }

            return pythonPath;
        }
    }
}
