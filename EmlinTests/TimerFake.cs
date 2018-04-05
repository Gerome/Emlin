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
        

        public bool Enabled { get; set; }

        public int Interval { get; set; }

        public void Dispose()
        {

        }

        public void AddToElapsed(long newTimeElapsed, CurrentSession testSession)
        {
            timeElapsed += newTimeElapsed;
            if(timeElapsed/TimeSpan.TicksPerMillisecond >= ConstantValues.LENGTH_OF_SESSION_IN_MILLIS)
            {
                testSession.End();
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
