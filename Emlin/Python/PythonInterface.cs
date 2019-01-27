

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Emlin.Python
{
    public class PythonInterface
    {
        // full path of python interpreter  
        private string python = @"C:\Users\Gerome\AppData\Local\Programs\Python\Python37-32\python.exe"; // TODO REMOVE THIS GARBAGE

        public void TeachModel()
        {
            // python app to call  
            string myPythonApp = "\"" + Environment.CurrentDirectory +  @"\..\..\Python\CreateSVM.py" + "\"";



            // Create new process start info 
            ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(python)
            {

                // make sure we can read the output from stdout 
                UseShellExecute = false,
                RedirectStandardOutput = true,

                Arguments = myPythonApp
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

        public void TestUserInput(List<string> testData)
        {
            string myPythonApp = "\"" + Environment.CurrentDirectory + @"\..\..\Python\LoadSVM.py" + "\"";


            string data = "";

            foreach(string line in testData)
            {
                data += line + ".";
            }

            data = data.Remove(data.Length - 1);

            // Create new process start info 
            ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(python)
            {

                // make sure we can read the output from stdout 
                UseShellExecute = false,
                RedirectStandardOutput = true,

                Arguments = myPythonApp + " " + data
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

            int user1 = 0, user6 = 0, user10 = 0;

            while (myString != null)
            {
                if (myString.Equals("[1]"))
                {
                    user1++;
                }
                if (myString.Equals("[6]"))
                {
                    user6++;
                }
                if (myString.Equals("[10]"))
                {
                    user10++;
                }
                Console.WriteLine(myString);
                myString = myStreamReader.ReadLine();
            }
            Console.WriteLine($"1 pressed {user1} times.");
            Console.WriteLine($"6 pressed {user6} times.");
            Console.WriteLine($"10 pressed {user10} times.");
        }
    }
}
