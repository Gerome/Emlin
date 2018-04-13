using NUnit.Framework;
using Emlin;
using System.Collections.Generic;
using System;

namespace EmlinTests
{
    class MergeListTest
    {

        List<KeyCombination> firstList = new List<KeyCombination>();
        List<KeyCombination> secondList = new List<KeyCombination>();

        KeyCombination keyComb0 = new KeyCombination(0);
        KeyCombination keyComb1 = new KeyCombination(1);
        KeyCombination keyComb2 = new KeyCombination(2);

        List<KeyCombination> returnList;

        [SetUp]
        public void SetUp()
        {
            returnList = new List<KeyCombination>();
            firstList = new List<KeyCombination>();
            secondList = new List<KeyCombination>();
        }

        [Test]
        public void MERGE_LIST_SHOULD_MERGE_TWO_EMPTY_LISTS()
        {
            MergeLists();
            Assert.That(returnList, Is.Not.Null);
        }

        [Test]
        public void MERGE_LIST_SHOULD_MERGE_TWO_LIST_WHEN_THE_FIRST_HAS_AN_OBJECT()
        {
            firstList.Add(keyComb0);
            MergeLists();
            Assert.That(returnList[0], Is.EqualTo(keyComb0));
        }

        [Test]
        public void MERGE_LIST_SHOULD_MERGE_TWO_LIST_WHEN_THE_SECOND_HAS_AN_OBJECT()
        {
            secondList.Add(keyComb1);
            MergeLists();
            Assert.That(returnList[0], Is.EqualTo(keyComb1));
        }
        

        private void MergeLists()
        {
            returnList = MergeList.MergeKeyboardCombinationList(firstList, secondList);
        }

        [Test]
        [Ignore("Test list")]
        public void DISABLED_TEST_LIST()
        {

        }
    }
}
