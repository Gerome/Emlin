using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emlin
{
    public class KeysData
    {
        public char FirstChar { get; set; }
        public char SecondChar { get; set; }

        public int CombinationID { get; set; } = 0;
        public TimeSpan HoldTime { get; set; } = new TimeSpan(0);
        public TimeSpan FlightTime { get; set; } = new TimeSpan(0);
        public TimeSpan Digraph1 { get; set; } = new TimeSpan(0);
        public TimeSpan Digraph2 { get; set; } = new TimeSpan(0);
        public TimeSpan Digraph3 { get; set; } = new TimeSpan(0);
    }
}
