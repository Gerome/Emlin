
using System;
using System.Timers;

namespace Emlin
{
    public interface ITimerInterface
    {
        event ElapsedEventHandler Elapsed;

        bool Enabled { get; set; }

        double Interval { get; set; }

        void Dispose();

        void Start();

        void Stop();

        void AddToElapsed(long timeToWait);
    }
}
