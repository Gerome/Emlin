using Emlin.Encryption;
using Emlin.Python;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Emlin
{
    public partial class Emlin : Form
    {
        private static DataFormatter dataFormatter;
        private HealthSubject health;

        #region lower level stuff
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private static IntPtr _hookID = IntPtr.Zero;
        private LowLevelKeyboardProc _proc;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        public static extern int GetKeyState(int nVirtKey);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        #endregion
        DevWindow devWindow = new DevWindow();

        public Emlin()
        {
            _proc = HookCallback;         
            _hookID = SetHook(_proc);

            
            InitializeComponent();

            CustomTimer timer = new CustomTimer(ConstantValues.LENGTH_OF_SESSION_IN_MILLIS);
            timer.Elapsed += TimerCountdown;
            timer.AutoReset = false;
            dataFormatter = new DataFormatter(timer);

#if DEBUG
            devWindow.Show();
#endif
            health = new HealthSubject();
            healthGraphView.SetHealthSubject(health);

            health.Attach(healthGraphView);
            health.SetValue(100);
        }

        private void TimerCountdown(object sender, EventArgs e)
        {
            string filepath = Path.Combine(ConstantValues.KEYBOARD_DATA_FILEPATH, ConstantValues.KEYBOARD_FILE_NAME);

            dataFormatter.RemoveLastDataItem();
            List<KeysData> dataToWriteToFile = dataFormatter.DataRecorded;

            if (dataToWriteToFile.Count != 0)
            {
                PythonInterface pi = new PythonInterface();

                List<string> formattedData = new List<string>();
                foreach (KeysData keysData in dataToWriteToFile)
                {
                    formattedData.Add(DataFormatter.GetFormattedDataLine(keysData));
                }
                pi.TestUserInput(formattedData, health);

                WriteEncryptedDataToFile(filepath, dataToWriteToFile);
            }

            PossiblyShuffleLines(filepath);

            dataFormatter.End();
        }

        private void PossiblyShuffleLines(string filepath)
        {
            // 1 in 100 chance of shuffling all the lines
            if(new Random().Next(1,100) == 42)
            {
                var lines = File.ReadAllLines(filepath);
                var rnd = new Random();
                lines = lines.OrderBy(line => rnd.Next()).ToArray();
                File.WriteAllLines(filepath, lines);
            }       
        }

        private static void WriteEncryptedDataToFile(string filepath, List<KeysData> dataToWriteToFile)
        {
            DataToFileWriter dtfw = new DataToFileWriter();
            dtfw.CreateDirectoryAndFile(filepath);
            dtfw.WriteRecordedDataToFile(dataToWriteToFile, filepath, new Encryptor());
        }

        private void SendKeyPressToCurrentSession(char charPressed, long timeInTicks)
        {
            WriteToDebugWindow(charPressed.ToString() + " pressed at " + new TimeSpan(timeInTicks).TotalMilliseconds.ToString());
            lock (dataFormatter)
            {
                dataFormatter.KeyWasPressed(charPressed, timeInTicks);
            }
        }

        private void SendKeyReleaseToCurrentSession(char charReleased, long timeInTicks)
        {
            WriteToDebugWindow(charReleased.ToString() + " released at " + new TimeSpan(timeInTicks).TotalMilliseconds.ToString());
            if (!OnlyKeyUpEvent(charReleased))
            {
                lock (dataFormatter)
                {
                    dataFormatter.KeyWasReleased(charReleased, timeInTicks);
                }
            }
        }

        private bool OnlyKeyUpEvent(char charReleased)
        {
            return charReleased == 164
                || charReleased == 165;
        }


        #region hook methods

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            int VK_SHIFT = 0x10;
            int VK_CAPS = 0X14;

            if (RecordingPossible(nCode, wParam))
            {
                int value = Marshal.ReadInt32(lParam);
                char key = (char)value;

                int capsState = GetKeyState(VK_CAPS) & 0x0001;

                if (capsState != 1 && (value >= 65 && value <= 90))
                {
                    int shiftState = GetKeyState(VK_SHIFT) & 0x0001;

                    key = char.ToLower(key);
                }

                if (wParam == (IntPtr)WM_KEYDOWN)
                {
                    SendKeyPressToCurrentSession(key, DateTime.Now.Ticks);
                }
                else
                {
                    SendKeyReleaseToCurrentSession(key, DateTime.Now.Ticks);
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private bool RecordingPossible(int nCode, IntPtr wParam)
        {
            return nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_KEYUP) && recordingEnabled.Checked;
        }

        #endregion

  
        private void WriteToDebugWindow(string output)
        {
            devWindow.textBox1.AppendText(output + Environment.NewLine);
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
            notifyIcon1.Visible = true;
        }

        private void deleteDataBtn_Click(object sender, EventArgs e)
        {
            string warning = "You are about to delete all your recorded data. Are you sure you want to do this?";
            DialogResult areYouSureDeleteDR = MessageBox.Show(warning, "Delete Recorded Data", MessageBoxButtons.YesNo);

            if(areYouSureDeleteDR == DialogResult.Yes)
            {
                string filepath = ConstantValues.KEYBOARD_DATA_FILEPATH + "\\" + ConstantValues.KEYBOARD_FILE_NAME;
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);

                    string dataDeleted = "Your data was succesfully deleted.";
                    MessageBox.Show(dataDeleted, "Data Deleted", MessageBoxButtons.OK);
                }
                else
                {
                    string dataNotDeleted = "Could not find file to delete.";

                    MessageBox.Show(dataNotDeleted, "File Not Found", MessageBoxButtons.OK);

                }
            }     
        }

        private void GoToDataBtn_Click(object sender, EventArgs e)
        {
            Process.Start(ConstantValues.KEYBOARD_DATA_FILEPATH + "\\");
        }
    }
}
