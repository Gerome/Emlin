
using System;
using System.Timers;

namespace Emlin
{
    public interface ITimerInterface
    {

        bool Enabled { get; set; }

        double Interval { get; set; }

        void Dispose();

        void Start();

        void Stop();
        
    }
}
