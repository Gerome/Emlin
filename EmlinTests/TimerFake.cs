using Emlin;
using System;
using System.Timers;

namespace EmlinTests
{
    public class TimerFake : ITimerInterface
    {
        private long timeElapsed;

        public TimerFake()
        {
            timeElapsed = 0;
        }

        public event ElapsedEventHandler Elapsed;

        public bool Enabled { get; set; }

        public double Interval { get; set; }

        public void Dispose()
        {

        }

        public void AddToElapsed(long newTimeElapsed)
        {
            timeElapsed += newTimeElapsed;
            if(timeElapsed/TimeSpan.TicksPerMillisecond >= ConstantValues.LENGTH_OF_SESSION_IN_MILLIS)
            {
                this.Elapsed.Invoke(this,null);
            }
        }

        public void Start()
        {
            timeElapsed = 0;
        }


        public void Stop()
        {

        }
    }
}
