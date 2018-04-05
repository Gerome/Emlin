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
            int lengthOfBothLists = firstList.Count + secondList.Count;

            List<KeyCombination> returnList = new List<KeyCombination>(lengthOfBothLists);

            int firstListCounter = 0;
            int secondListCounter = 0;

            for(int i = 0; i < lengthOfBothLists; i++)
            {
                if (firstList.Count <= firstListCounter || secondList.Count <= secondListCounter) {
                    if (firstList.Count <= firstListCounter)
                    {
                        returnList.Add(secondList[secondListCounter]);
                        secondListCounter++;
                    }
                    else if (secondList.Count <= secondListCounter)
                    {
                        returnList.Add(firstList[firstListCounter]);
                        firstListCounter++;
                    }
                }
                else {
                  

                    if (firstList[firstListCounter].CombId < secondList[secondListCounter].CombId)
                    {
                        returnList.Add(firstList[firstListCounter]);
                        firstListCounter++;
                    }
                    else if (firstList[firstListCounter].CombId > secondList[secondListCounter].CombId)
                    {
                        returnList.Add(secondList[secondListCounter]);
                        secondListCounter++;
                    }
                    else if(firstList[firstListCounter].CombId == secondList[secondListCounter].CombId)
                    {
                        
                        foreach(TimeSpan ts in secondList[secondListCounter].TimeSpanList)
                        {
                            firstList[firstListCounter].AddTimespanToList(ts);
                        }

                        returnList.Add(firstList[firstListCounter]);
                        firstListCounter++;
                        secondListCounter++;
                        i++;
                    }
                }
            }

            return returnList;
        }
    }
}
