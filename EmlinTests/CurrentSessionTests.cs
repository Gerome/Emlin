using Emlin;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmlinTests
{
    class CurrentSessionTests
    {
        TimerFake timerFake;

        CurrentSession testSession;

        long timeElapsed;

        [SetUp]
        public void Init()
        {
            timerFake = new TimerFake();
            testSession = new CurrentSession(timerFake);
            timeElapsed = 0;
        }

        [TearDown]
        public void Dispose()
        {
            testSession.End();
        }

        [Test]
        public void PRESSING_TWO_KEYS_ADDS_ONE_COMBINATION_TO_THE_TIMESPAN_LIST()
        {
            PressKey('A');
            PressKey('B');
            int combId = HelperFunctions.GetCombinationId('A', 'B');
            Assert.That(testSession.KeysPressed[0].TimeSpanList.Count, Is.EqualTo(1));
        }

        [Test]
        public void CURRENT_SESSION_IS_INACTIVE_BY_DEFAULT()
        {
            Assert.That(testSession.CurrentState, Is.EqualTo(CurrentSession.SessionState.Inactive));
        }

        [Test]
        public void PRESSING_A_KEY_SETS_THE_STATE_OF_THE_SESSION_TO_ACTIVE()
        {
            PressKey('A');
            Assert.That(testSession.CurrentState, Is.EqualTo(CurrentSession.SessionState.Active));
        }

        [Test]
        public void PRESSING_2_KEY_SETS_THE_STATE_OF_THE_SESSION_TO_ACTIVE()
        {
            PressKey('A');
            Wait(200);
            PressKey('B');
            Assert.That(testSession.CurrentState, Is.EqualTo(CurrentSession.SessionState.Active));
        }

        [Test]
        public void PRESSING_3_KEY_WITH_1_SECOND_DELAY_KEEPS_SESSION_ACTIVE()
        {
            PressKey('A');
            Wait(SecondsToTicks(1));
            PressKey('B');
            Wait(SecondsToTicks(1));
            PressKey('C');
            Wait(SecondsToTicks(1));
            Assert.That(testSession.CurrentState, Is.EqualTo(CurrentSession.SessionState.Active));
        }

        [Test]
        public void PRESSING_A_KEY_AND_WAITING_2_SECONDS_SHOWS_THE_SESSION_IS_INACTIVE()
        {
            PressKey('A');
            Assert.That(testSession.CurrentState, Is.EqualTo(CurrentSession.SessionState.Active));
            Wait(SecondsToTicks(2));
            Assert.That(testSession.CurrentState, Is.EqualTo(CurrentSession.SessionState.Inactive));
        }

        [Test]
        public void PRESSING_TWO_KEYS_ADDS_A_COMBINATION_OBJECT_TO_THE_CURRENT_SESSION_COMBINATION_LIST()
        {
            PressKey('A');
            Wait(100);
            PressKey('B');

            int combId = HelperFunctions.GetCombinationId('A', 'B');

            Assert.That(testSession.KeysPressed[0].TimeSpanList[0].Ticks, Is.EqualTo(100));
        }

        [Test]
        public void PRESSING_THREE_KEYS_ADDS_A_COMBINATION_OBJECT_TO_THE_CURRENT_SESSION_COMBINATION_LIST()
        {
            PressKey('A');
            Wait(100);
            PressKey('B');
            Wait(100);
            PressKey('C');

            int firstCombId = HelperFunctions.GetCombinationId('A', 'B');
            int secondCombId = HelperFunctions.GetCombinationId('B', 'C');

            Assert.That(testSession.KeysPressed[0].TimeSpanList[0].Ticks, Is.EqualTo(100));
            Assert.That(testSession.KeysPressed[1].TimeSpanList[0].Ticks, Is.EqualTo(100));
        }

        [Test]
        public void PRESSING_TWO_KEYS_WITH_A_2_SECOND_GAP_FINISHES_THE_SESSION()
        {
            PressKey('A');
            Wait(SecondsToTicks(2));
            PressKey('B');
            int combId = HelperFunctions.GetCombinationId('A', 'B');

            Assert.That(testSession.KeysPressed, Is.Empty);
        }

        [Test]
        public void PRESSING_THREE_OF_THE_SAME_KEY_ADDS_A_COMBINATION_OBJECT_TO_THE_CURRENT_SESSION_COMBINATION_LIST_WITH_TWO_IN_THE_TIMESPAN_LIST()
        {
            PressKey(' ');
            Wait(100);
            PressKey(' ');
            Wait(100);
            PressKey(' ');

            int combId = HelperFunctions.GetCombinationId(' ', ' ');

            Assert.That(testSession.KeysPressed[0].TimeSpanList.Count, Is.EqualTo(2));
        }

        [Test]
        public void PRESSING_THREE_COMBINATIONS_CREATES_THREE_OBJECTS()
        {
            PressKey(' ');
            Wait(100);
            PressKey(' ');
            Wait(100);
            PressKey('a');
            Wait(100);
            PressKey('a');

            int firstCombId = HelperFunctions.GetCombinationId(' ', ' ');
            int secondCombId = HelperFunctions.GetCombinationId(' ', 'a');
            int thirdCombId = HelperFunctions.GetCombinationId('a', 'a');


            Assert.That(testSession.KeysPressed.Count, Is.EqualTo(3));
            Assert.That(testSession.KeysPressed[0].TimeSpanList.Count, Is.EqualTo(1));
            Assert.That(testSession.KeysPressed[1].TimeSpanList.Count, Is.EqualTo(1));
            Assert.That(testSession.KeysPressed[2].TimeSpanList.Count, Is.EqualTo(1));
        }

        private void PressKey(char charPressed)
        {
            testSession.KeyWasPressed(charPressed, timeElapsed);
        }

        private void Wait(long timeToWait)
        {
            timeElapsed += timeToWait;
            timerFake.AddToElapsed(timeToWait, testSession);
        }

        private long SecondsToTicks(int seconds)
        {
            return seconds * TimeSpan.TicksPerSecond;
        }
    }
}
