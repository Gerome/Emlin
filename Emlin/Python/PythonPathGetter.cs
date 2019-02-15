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


            return Path.Combine(Environment.CurrentDirectory, @"Python\python.exe");
        }
    }
}
