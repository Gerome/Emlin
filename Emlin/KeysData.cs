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

        public int CombinationID { get; set; }
        public TimeSpan HoldTime { get; set; }
        public TimeSpan FlightTime { get; set; }
        public TimeSpan Digraph1 { get; set; }
        public TimeSpan Digraph2 { get; set; }
        //public TimeSpan Digraph3 { get; set; }

    }
}
