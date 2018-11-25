using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Emlin
{
    public partial class EmlinView : Form
    {
        private static DataFormatter dataFormatter;

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

        DevWindow devWindow = new DevWindow();

        public EmlinView()
        {
            _proc = this.HookCallback;

            this.WindowState = FormWindowState.Minimized;

            InitializeComponent();

            _hookID = SetHook(_proc);

            CustomTimer timer = new CustomTimer(ConstantValues.LENGTH_OF_SESSION_IN_MILLIS);
            timer.Tick += TimerCountdown;
            

            dataFormatter = new DataFormatter(timer);

#if DEBUG
            devWindow.Show();
#endif
        }

#region methods

        void TimerCountdown(object sender, EventArgs e)
        {
            string filepath = ConstantValues.KEYBOARD_DATA_FILEPATH + @"\KeyboardData.txt";
            
            List<KeysData> dataToWriteToFile = dataFormatter.DataRecorded;

            if (dataToWriteToFile.Count != 0)
            {        
                DataToFileWriter dtfw = new DataToFileWriter();
                dtfw.CreateDirectoryAndFile(filepath);
                
                dtfw.WriteRecordedDataToFile(dataToWriteToFile, filepath);
            }

            dataFormatter.End();
        }

        private void SendKeyPressToCurrentSession(char charPressed, long timeInTicks)
        {
            devWindow.textBox1.AppendText(charPressed.ToString() + " pressed at " + (new TimeSpan(timeInTicks).Milliseconds).ToString() + Environment.NewLine);
            dataFormatter.KeyWasPressed(charPressed, timeInTicks);  
        }

        private void SendKeyReleaseToCurrentSession(char charReleased, long timeInTicks)
        {
            devWindow.textBox1.AppendText(charReleased.ToString() + " released at " + (new TimeSpan(timeInTicks).Milliseconds).ToString() + Environment.NewLine);
            if (!OnlyKeyUpEvent(charReleased))
            {
                dataFormatter.KeyWasReleased(charReleased, timeInTicks);
            }
        }

        private bool OnlyKeyUpEvent(char charReleased)
        {
            return charReleased == 164
                || charReleased == 165;
        }

        #endregion

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

            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_KEYUP))
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

#endregion

        private void EmlinView_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = true;
        }
    }
}
