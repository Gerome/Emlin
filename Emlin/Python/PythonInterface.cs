

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Emlin.Python
{
    public class PythonInterface
    {
        public void TeachModel(string model, HealthSubject health)
        {
            // python app to call  
            string myPythonApp = "";

            switch (model)
            {
                case ("KNN"):
                    myPythonApp = "\"" + Environment.CurrentDirectory + @"\Python\KNN\CreateKNN.py" + "\"";
                    break;

                case ("SVM"):
                    myPythonApp = "\"" + Environment.CurrentDirectory + @"\Python\SVM\CreateSVM.py" + "\"";
                    break;

                default:
                    return;
            }

            RunPython(myPythonApp, health);
        }

        public void TestUserInput(List<string> testData, HealthSubject health = null)
        {
            string myPythonApp = "\"" + Environment.CurrentDirectory + @"\Python\KNN\LoadKNN.py" + "\"";


            string data = "";

            foreach (string line in testData)
            {
                data += line + ";";
            }

            data = data.Remove(data.Length - 1);
            RunPython(myPythonApp, health, data);
        }

        public void GenerateNonUserData()
        {
            
        }

        private void RunPython(string myPythonApp, HealthSubject health = null, string data = "")
        {
            string pythonExePath = PythonPathGetter.GetPythonExePath();

            if (pythonExePath.Equals(""))
            {

                Environment.Exit(0);
            }

            // Create new process start info 
            ProcessStartInfo myProcessStartInfo = new ProcessStartInfo()
            {

                // make sure we can read the output from stdout 
                FileName = pythonExePath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
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

                PrintPythonOutput(myProcess, health);
                // wait exit signal from the app we called 
                myProcess.WaitForExit();

                // close the process 
                myProcess.Close();
            }).Start();
        }

        private static void PrintPythonOutput(Process myProcess, HealthSubject health)
        {
            // Read the standard output of the app we called.  
            StreamReader myStreamReader = myProcess.StandardOutput;
          
            string myString;

            int user = 0, notUser = 0;

            while((myString = myStreamReader.ReadLine()) != null)
            {
                Console.WriteLine(myString);
                if (myString.Equals("[1]"))
                {
                    user++;
                }
                if (myString.Equals("[0]"))
                {
                    notUser++;       
                }
            }

            int changeInHealth = (user - notUser);
            Console.WriteLine(changeInHealth);
            health.SetValue(health.GetValue() + changeInHealth);
            

            Console.WriteLine($"User pressed {user} times.");
            Console.WriteLine($"Not user pressed {notUser} times.");
        }
    }
}
