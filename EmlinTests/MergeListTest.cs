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
        public void MERGE_LIST_SHOULD_RETURN_A_LIST_WHEN_COMBINATION_IDS_ARE_THE_SAME()
        {
            List<KeyCombination> firstList = new List<KeyCombination>();
            List<KeyCombination> secondList = new List<KeyCombination>();

            firstList.Add(new KeyCombination(0));

            secondList.Add(new KeyCombination(0));

            List<KeyCombination> returnList = MergeList.MergeKeyboardCombinationList(firstList, secondList);

            Assert.That(returnList[0].CombId, Is.EqualTo(0));
            Assert.That(returnList[1].CombId, Is.EqualTo(0));

        }
    }
}
