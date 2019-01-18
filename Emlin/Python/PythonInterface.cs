

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Emlin.Python
{
    static class PythonInterface
    {
        public static void TestRunPython()
        {
            // full path of python interpreter  
            string python = @"C:\Users\Gerome\AppData\Local\Programs\Python\Python37-32\python.exe";

            // python app to call  
            string myPythonApp = "\"C:\\Users\\Gerome\\Dropbox\\CI301-The Individual Project\\Emlin\\Emlin\\Python\\EmlinSVM.py\"";



            // Create new process start info 
            ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(python)
            {

                // make sure we can read the output from stdout 
                UseShellExecute = false,
                RedirectStandardOutput = true,

                Arguments = myPythonApp + ""
            };


            Process myProcess = new Process
            {
                // assign start information to the process 
                StartInfo = myProcessStartInfo
            };

            new Thread(() =>
            {
                // start process 
                myProcess.Start();

                PrintPythonOutput(myProcess);
                // wait exit signal from the app we called 
                myProcess.WaitForExit();

                // close the process 
                myProcess.Close();
            }).Start();
        }

        private static void PrintPythonOutput(Process myProcess)
        {
            // Read the standard output of the app we called.  
            StreamReader myStreamReader = myProcess.StandardOutput;
            string myString = myStreamReader.ReadLine();

            while (myString != null)
            {
                Console.WriteLine(myString);
                myString = myStreamReader.ReadLine();
            }
        }
    }
}
