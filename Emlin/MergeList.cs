using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emlin
{
    public static class MergeList
    {
        public static List<KeyCombination> MergeKeyboardCombinationList(List<KeyCombination> firstList, List<KeyCombination> secondList)
        {
            List<KeyCombination> returnList = new List<KeyCombination>();

            foreach(KeyCombination keyComb in firstList)
            {
                returnList.Add(firstList[0]);
            }

            foreach (KeyCombination keyComb in secondList)
            {
                returnList.Add(secondList[0]);
            }

            return returnList;
        }
    }
}
