
namespace Emlin
{
    public class HealthSubject : BaseSubject
    {
        private int Value;

        public int GetValue()
        {
            return Value;
        }

        public void SetValue(int value)
        {
            if (value >= 100)
            {
                Value = 100;
            }
            else
            {
                Value = value;
            }

            Notify();

            if(Value < 0)
            {
                System.Diagnostics.Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
            }
        }
    }
}
