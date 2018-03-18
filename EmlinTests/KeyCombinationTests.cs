using System;
using NUnit.Framework;
using Emlin;

namespace EmlinTests
{
    class KeyCombinationTests
    {
        [Test]
        public void PRESSING_TWO_KEYS_CREATES_COMBINATION_OBJECT()
        {
            int combId = HelperFunctions.GetCombinationId('A', 'B');
            KeyCombination keyComb = new KeyCombination(combId);
            keyComb.AddTimespanToList(new TimeSpan(300000));

            Assert.That(keyComb.TimeSpanList[0].Ticks, Is.EqualTo(300000));
        }
    }
}
