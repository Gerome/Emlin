using Emlin;
using NUnit.Framework;
using System;
using System.Linq;

namespace EmlinTests
{
    class DataFormatterTests
    {
        TimerFake timerFake;

        DataFormatter testSession;

        long timeElapsed;

        [SetUp]
        public void Init()
        {
            timerFake = new TimerFake();
            testSession = new DataFormatter(timerFake);
            timeElapsed = 0;
        }

        [TearDown]
        public void Dispose()
        {
            testSession.End();
        }

        [Test]
        public void CURRENT_SESSION_IS_INACTIVE_BY_DEFAULT()
        {
            Assert.That(testSession.CurrentState, Is.EqualTo(DataFormatter.SessionState.Inactive));
        }

        [Test]
        public void PRESSING_A_KEY_SETS_THE_STATE_OF_THE_SESSION_TO_ACTIVE()
        {
            PressKey('A');
            Assert.That(testSession.CurrentState, Is.EqualTo(DataFormatter.SessionState.Active));
        }

        [Test]
        public void PRESSING_2_KEY_SETS_THE_STATE_OF_THE_SESSION_TO_ACTIVE()
        {
            PressKey('A');
            Wait(200);
            PressKey('B');
            Assert.That(testSession.CurrentState, Is.EqualTo(DataFormatter.SessionState.Active));
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
            Assert.That(testSession.CurrentState, Is.EqualTo(DataFormatter.SessionState.Active));
        }

        [Test]
        public void PRESSING_A_KEY_AND_WAITING_2_SECONDS_SHOWS_THE_SESSION_IS_INACTIVE()
        {
            PressKey('A');
            Assert.That(testSession.CurrentState, Is.EqualTo(DataFormatter.SessionState.Active));
            Wait(SecondsToTicks(2));
            Assert.That(testSession.CurrentState, Is.EqualTo(DataFormatter.SessionState.Inactive));
        }
        
        [Test]
        public void Pressing_and_Releasing_a_key_should_record_the_Hold_Time()
        {
            PressKey('a');
            Wait(100);
            ReleaseKey('a');

            Assert.That(testSession.DataRecorded[0].HoldTime.Ticks, Is.EqualTo(100));
        }

        [Test]
        public void Pressing_and_Releasing_2_keys_should_record_the_Hold_Time_of_each()
        {
            PressKey('a');
            Wait(100);
            ReleaseKey('a');

            PressKey('b');
            Wait(200);
            ReleaseKey('b');

            Assert.That(testSession.DataRecorded[0].HoldTime.Ticks, Is.EqualTo(100));
            Assert.That(testSession.DataRecorded[1].HoldTime.Ticks, Is.EqualTo(200));
        }

        [Test]
        public void Pressing_and_Releasing_the_same_2_keys_should_record_the_Hold_Time_of_each()
        {
            PressKey('a');
            Wait(100);
            ReleaseKey('a');

            PressKey('a');
            Wait(200);
            ReleaseKey('a');

            Assert.That(testSession.DataRecorded[0].HoldTime.Ticks, Is.EqualTo(100));
            Assert.That(testSession.DataRecorded[1].HoldTime.Ticks, Is.EqualTo(200));
        }

        [Test]
        public void Pressing_and_Releasing_2_keys_intermediately_should_still_record_the_Hold_Time_of_each()
        {
            PressKey('a');
            Wait(50);
            PressKey('b');
            Wait(100);
            ReleaseKey('a');
            Wait(200);
            ReleaseKey('b');

            Assert.That(testSession.DataRecorded[0].HoldTime.Ticks, Is.EqualTo(150));
            Assert.That(testSession.DataRecorded[1].HoldTime.Ticks, Is.EqualTo(300));
        }

        [Test]
        public void Pressing_a_key_and_another_should_record_the_combination_ID()
        {
            PressKey('a');
            PressKey('b');
            int combId = HelperFunctions.GetCombinationId('a', 'b');

            Assert.That(testSession.DataRecorded[0].CombinationID, Is.EqualTo(combId));
        }

        [Test]
        public void Pressing_3_keys_should_record_the_combination_IDs()
        {
            PressKey('a');
            PressKey('b');
            PressKey('c');
            int combId = HelperFunctions.GetCombinationId('a', 'b');
            Assert.That(testSession.DataRecorded[0].CombinationID, Is.EqualTo(combId));

            combId = HelperFunctions.GetCombinationId('b', 'c');
            Assert.That(testSession.DataRecorded[1].CombinationID, Is.EqualTo(combId));
        }

        [Test]
        public void Holding_down_a_key_should_not_crash_the_program()
        {
            PressKey('a');
            PressKey('a');
            PressKey('a');
            PressKey('a');
            PressKey('a');
            PressKey('a');
        }

        [Test]
        public void Holding_down_a_key_means_session_should_not_end()
        {
            PressKey('a');
            Wait(15000);
            Assert.That(testSession.CurrentState, Is.EqualTo(DataFormatter.SessionState.Active));
        }

        /*
         * Test list
         * 
         * Hold time
         *  pressing and releasing 2 key should record the hold time of each
         *  
         * Flight time
         *  releasing a key and pressing another should record the flight time
         *  pressing a key, pressing another and releasing should record a negative flight time
         *  
         * Digraph
         *  pressing a key and pressing another should record the Di1
         *  releasing a key and releasing another should record the Di2
         *  pressing a key and releasing another should record the Di3
         */


        private void PressKey(char charPressed)
        {
            testSession.KeyWasPressed(charPressed, timeElapsed);
        }

        private void Wait(long timeToWait)
        {
            timeElapsed += timeToWait;
            timerFake.AddToElapsed(timeToWait, testSession);
        }

        private void ReleaseKey(char charReleased)
        {
            testSession.KeyWasReleased(charReleased, timeElapsed);
        }

        private long SecondsToTicks(int seconds)
        {
            return seconds * TimeSpan.TicksPerSecond;
        }
    }
}
