using System;
using System.Collections.Generic;

namespace Emlin
{
    public class KeysData
    {
        public char FirstChar { get; set; }
        public char SecondChar { get; set; }


        public enum WaitingForEvent
        {
            EFirstKeyRelease,
            ESecondKeyPress,
            ESecondKeyRelease
        }

        public List<WaitingForEvent> WaitingForEvents = new List<WaitingForEvent> { WaitingForEvent.EFirstKeyRelease, WaitingForEvent.ESecondKeyPress, WaitingForEvent.ESecondKeyRelease };

        public int CombinationID { get; set; }
        public TimeSpan HoldTime { get; set; }
        public TimeSpan FlightTime { get; set; }
        public TimeSpan Digraph1 { get; set; }
        public TimeSpan Digraph2 { get; set; }
        public TimeSpan Digraph3 { get; set; }
    }
}
