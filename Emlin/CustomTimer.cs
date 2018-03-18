using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Emlin
{
    class CustomTimer : Timer, ITimerInterface
    {

        public CustomTimer(double interval)
        {
            this.Interval = interval;
        }

        public void AddToElapsed(long whatever)
        {

        }
    }
}
