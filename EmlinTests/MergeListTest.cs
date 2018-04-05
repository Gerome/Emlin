using NUnit.Framework;
using Emlin;
using System.Collections.Generic;
using System;

namespace EmlinTests
{
    class MergeListTest
    {

        [Test]
        public void MERGE_LIST_SHOULD_SORT_THE_TWO_LISTS_AND_RETURN_A_LIST()
        {
            List<KeyCombination> firstList = new List<KeyCombination>();
            List<KeyCombination> secondList = new List<KeyCombination>();

            firstList.Add(new KeyCombination(0));
            firstList.Add(new KeyCombination(2));

            secondList.Add(new KeyCombination(1));

            List<KeyCombination> returnList = MergeList.MergeKeyboardCombinationList(firstList, secondList);

            Assert.That(returnList[0].CombId, Is.EqualTo(0));
            Assert.That(returnList[1].CombId, Is.EqualTo(1));
            Assert.That(returnList[2].CombId, Is.EqualTo(2));

        }


        [Test]
        public void MERGE_LIST_SHOULD_JOIN_TWO_KEY_COMBINATIONS_WITH_THE_SAME_COMBINATION_ID()
        {
            List<KeyCombination> firstList = new List<KeyCombination>();
            List<KeyCombination> secondList = new List<KeyCombination>();


            KeyCombination firstKeyComb = new KeyCombination(0);
            firstKeyComb.AddTimespanToList(new TimeSpan(20));

            KeyCombination secondKeyComb = new KeyCombination(0);
            secondKeyComb.AddTimespanToList(new TimeSpan(10));

            firstList.Add(firstKeyComb);
            secondList.Add(secondKeyComb);

            List<KeyCombination> returnList = MergeList.MergeKeyboardCombinationList(firstList, secondList);

            Assert.That(returnList[0].TimeSpanList[0].Ticks, Is.EqualTo(20));
            Assert.That(returnList[0].TimeSpanList[1].Ticks, Is.EqualTo(10));

        }
    }
}
