using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emlin
{
    public class KeyCombination
    {
        private int combId;
        private List<TimeSpan> timeSpanList;

        public int CombId { get => combId; set => combId = value; }
        public List<TimeSpan> TimeSpanList { get => timeSpanList; }


        public KeyCombination(int combId)
        {
            CombId = combId;
            timeSpanList = new List<TimeSpan>();
        }

        public void AddTimespanToList(TimeSpan timeSpan)
        {
            TimeSpanList.Add(timeSpan);
        }

    }
}
