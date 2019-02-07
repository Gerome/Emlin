using System.Timers;

namespace Emlin
{
    class CustomTimer : Timer, ITimerInterface
    {
        public CustomTimer(int interval)
        {
            Interval = interval;
        }
    }
}
