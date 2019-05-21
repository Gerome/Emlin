

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Emlin.Python
{
    public class PythonInterface
    {
        HealthSubject health;

        public void TeachModel(string model, string dataDirectoryPath)
        {
            // python app to call  
            string myPythonApp = "";

            switch (model)
            {
                case ("RFT"):
                    myPythonApp = "\"" + Environment.CurrentDirectory + @"\Python\RFT\CreateRFT.py" + "\"";
                    break;

                case ("KNN"):
                    myPythonApp = GetPythonFilePath("CreateKNN");
                    break;

                case ("SVM"):
                    myPythonApp = "\"" + Environment.CurrentDirectory + @"\Python\SVM\CreateSVM.py" + "\"";
                    break;

                default:
                    return;
            }

            RunPython(myPythonApp, dataDirectoryPath);
        }

        public void TestUserInput(List<string> testData, HealthSubject health, string dataDirectoryPath)
        {
            this.health = health;
            string myPythonApp = GetPythonFilePath("LoadKNN");

            string data = "";

            foreach (string line in testData)
            {
                data += line + ";";
            }

            data = data.Remove(data.Length - 1);

            data += " " + dataDirectoryPath;
            RunPython(myPythonApp, data);
        }

        public void GenerateNonUserData(string dataDirectoryPath)
        {
            string inverseDataGeneratorPath = GetPythonFilePath("generate_inverse_data");
            RunPython(inverseDataGeneratorPath, dataDirectoryPath);
        }

        public void ProcessUserAndGeneratedData(string dataDirectoryPath)
        {
            string processedDataAppenderPath = GetPythonFilePath("compile_final_data_into_file");
            RunPython(processedDataAppenderPath, dataDirectoryPath);
        }

        private void RunPython(string myPythonApp, string data = "")
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

            
            // start process 
            myProcess.Start();

            PrintPythonOutput(myProcess);
            // wait exit signal from the app we called 
            myProcess.WaitForExit();

            // close the process 
            myProcess.Close();
            
        }

        private void PrintPythonOutput(Process myProcess)
        {
            // Read the standard output of the app we called.  
            StreamReader myStreamReader = myProcess.StandardOutput;
          
            string myString;

            int user = 0, notUser = 0;

            while((myString = myStreamReader.ReadLine()) != null)
            {
                Console.WriteLine(myString);
                if (myString.Contains("1"))
                {
                    user++;
                }
                if (myString.Contains("0"))
                {
                    notUser++;       
                }

                if (myString.Contains("ModuleNotFoundError"))
                {
                    ShowErrorWindow();
                    break;
                }
            }


            if(health != null)
            {
                int changeInHealth = (user - notUser);
                Console.WriteLine(changeInHealth);
                health.SetValue(health.GetValue() + changeInHealth);
                Console.WriteLine($"User pressed {user} times.");
                Console.WriteLine($"Not user pressed {notUser} times.");
            }
        }

        private static void ShowErrorWindow()
        {
            string warning = "You don't have the required Python libraries installed. Please run the \"PythonLibInstaller.bat\" in the install folder.";
            string title = "Error";
            MessageBox.Show(warning, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static string GetPythonFilePath(string pythonFileName)
        {
            return "\"" + Environment.CurrentDirectory + @"\Python\" + pythonFileName + ".py" + "\"";
        }
    }
}
