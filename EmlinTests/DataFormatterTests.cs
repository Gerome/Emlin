﻿using Emlin;
using NUnit.Framework;
using System;
using System.Linq;

namespace EmlinTests
{
    class DataFormatterTests
    {
        TimerFake timerFake;

        DataFormatter testFormattter;

        long timeElapsed;

        [SetUp]
        public void Init()
        {
            timerFake = new TimerFake();
            testFormattter = new DataFormatter(timerFake);
            timeElapsed = 0;
        }

        [TearDown]
        public void Dispose()
        {
            testFormattter.End();
        }

        [Test]
        public void Current_session_is_inactive_by_default()
        {
            Assert.That(testFormattter.CurrentState, Is.EqualTo(DataFormatter.SessionState.Inactive));
        }

        [Test]
        public void Pressing_a_key_sets_the_state_of_the_session_to_active()
        {
            PressKey('A');
            Assert.That(testFormattter.CurrentState, Is.EqualTo(DataFormatter.SessionState.Active));
        }

        [Test]
        public void Pressing_2_key_sets_the_state_of_the_session_to_active()
        {
            PressKey('A');
            Wait(200);
            PressKey('B');
            Assert.That(testFormattter.CurrentState, Is.EqualTo(DataFormatter.SessionState.Active));
        }

        [Test]
        public void Pressing_3_key_with_1_second_delay_keeps_session_active()
        {
            PressKey('A');
            Wait(SecondsToTicks(1));
            PressKey('B');
            Wait(SecondsToTicks(1));
            PressKey('C');
            Wait(SecondsToTicks(1));
            Assert.That(testFormattter.CurrentState, Is.EqualTo(DataFormatter.SessionState.Active));
        }

        [Test]
        public void Pressing_a_key_and_waiting_2_seconds_shows_the_session_is_inactive()
        {
            PressKey('A');
            Assert.That(testFormattter.CurrentState, Is.EqualTo(DataFormatter.SessionState.Active));
            Wait(SecondsToTicks(2));
            Assert.That(testFormattter.CurrentState, Is.EqualTo(DataFormatter.SessionState.Inactive));
        }
        
        [Test]
        public void Pressing_and_Releasing_a_key_should_record_the_Hold_Time()
        {
            Wait(100);
            PressKey('a');
            Wait(100);
            ReleaseKey('a');

            Assert.That(testFormattter.DataRecorded[0].HoldTime.Ticks, Is.EqualTo(100));
        }

        [Test]
        public void Pressing_and_Releasing_2_keys_should_record_the_Hold_Time_of_each()
        {
            Wait(100);
            PressKey('a');
            Wait(100);
            ReleaseKey('a');

            PressKey('b');
            Wait(200);
            ReleaseKey('b');

            Assert.That(testFormattter.DataRecorded[0].HoldTime.Ticks, Is.EqualTo(100));
            Assert.That(testFormattter.DataRecorded[1].HoldTime.Ticks, Is.EqualTo(200));
        }

        [Test]
        public void Pressing_and_Releasing_the_same_2_keys_should_record_the_Hold_Time_of_each()
        {
            Wait(100);
            PressKey('a');
            Wait(100);
            ReleaseKey('a');

            PressKey('a');
            Wait(200);
            ReleaseKey('a');

            Assert.That(testFormattter.DataRecorded[0].HoldTime.Ticks, Is.EqualTo(100));
            Assert.That(testFormattter.DataRecorded[1].HoldTime.Ticks, Is.EqualTo(200));
        }

        [Test]
        public void Pressing_and_Releasing_2_keys_intermediately_should_still_record_the_Hold_Time_of_each()
        {
            Wait(100);
            PressKey('a');
            Wait(50);

            PressKey('b');
            Wait(100);

            ReleaseKey('a');
            Wait(200);

            ReleaseKey('b');

            Assert.That(testFormattter.DataRecorded[0].HoldTime.Ticks, Is.EqualTo(150));
            Assert.That(testFormattter.DataRecorded[1].HoldTime.Ticks, Is.EqualTo(300));
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
        public void Holding_down_multiple_keys_should_not_crash_the_program()
        {
            PressKey('a');
            PressKey('b');
            PressKey('a');
            PressKey('b');
        }

        [Test]
        public void Holding_down_a_key_should_record_the_hold_time()
        {
            Wait(100);
            PressKey('a');
            Wait(100);
            PressKey('a');
            Wait(100);
            PressKey('a');
            Wait(100);
            PressKey('a');
            Wait(100);
            ReleaseKey('a');

            Assert.That(testFormattter.DataRecorded[0].HoldTime.Ticks, Is.EqualTo(400));
        }

        [Test]
        public void Pressing_a_key_and_another_should_record_the_combination_ID()
        {
            PressKey('a');
            PressKey('b');
            int combId = HelperFunctions.GetCombinationId('a', 'b');

            Assert.That(testFormattter.DataRecorded[0].CombinationID, Is.EqualTo(combId));
        }

        [Test]
        public void Pressing_3_keys_should_record_the_combination_IDs()
        {
            PressKey('a');
            PressKey('b');
            PressKey('c');
            int combId = HelperFunctions.GetCombinationId('a', 'b');
            Assert.That(testFormattter.DataRecorded[0].CombinationID, Is.EqualTo(combId));

            combId = HelperFunctions.GetCombinationId('b', 'c');
            Assert.That(testFormattter.DataRecorded[1].CombinationID, Is.EqualTo(combId));
        }

        [Test]
        public void Releasing_a_key_and_pressing_another_should_record_the_flight_time()
        {
            Wait(100);
            PressKey('a');
            Wait(100);
            ReleaseKey('a');
            Wait(50);
            PressKey('b');

            Assert.That(testFormattter.DataRecorded[0].FlightTime.Ticks, Is.EqualTo(50));
        }

        [Test]
        public void Pressing_two_keys_then_releasing_one_should_record_a_negative_flight_time()
        {
            Wait(100);
            PressKey('a');
            Wait(50);
            PressKey('b');
            Wait(25);
            ReleaseKey('a');

            Assert.That(testFormattter.DataRecorded[0].FlightTime.Ticks, Is.EqualTo(-25));
        }

        [Test]
        public void Pressing_two_keys_then_releasing_the_second_first_should_record_a_negative_flight_time()
        {
            Wait(100);
            PressKey('a');
            Wait(50);
            PressKey('b');
            Wait(25);
            ReleaseKey('b');
            Wait(25);
            ReleaseKey('a');

            Assert.That(testFormattter.DataRecorded[0].FlightTime.Ticks, Is.EqualTo(-50));
        }

        [Test]
        public void Holding_three_keys_and_releasing_in_order_record_Hold_and_Flight_time_correctly()
        {
            Wait(10);
            PressKey('a');
            Wait(10);
            PressKey('b');
            Wait(15);
            PressKey('c');
            Wait(15);
            ReleaseKey('a');
            Wait(25);
            ReleaseKey('b');
            Wait(30);
            ReleaseKey('c');

            Assert.That(testFormattter.DataRecorded[0].HoldTime.Ticks, Is.EqualTo(40));
            Assert.That(testFormattter.DataRecorded[0].FlightTime.Ticks, Is.EqualTo(-30));

            Assert.That(testFormattter.DataRecorded[1].HoldTime.Ticks, Is.EqualTo(55));
            Assert.That(testFormattter.DataRecorded[1].FlightTime.Ticks, Is.EqualTo(-40));
        }

        [Test]
        public void Holding_three_keys_and_releasing_not_ain_order_record_Hold_and_Flight_time_correctly()
        {
            Wait(10);
            PressKey('a');
            Wait(10);
            PressKey('b');
            Wait(15);
            PressKey('c');
            Wait(15);
            ReleaseKey('c');
            Wait(25);
            ReleaseKey('b');
            Wait(30);
            ReleaseKey('a');

            Assert.That(testFormattter.DataRecorded[0].HoldTime.Ticks, Is.EqualTo(95));
            Assert.That(testFormattter.DataRecorded[0].FlightTime.Ticks, Is.EqualTo(-85));

            Assert.That(testFormattter.DataRecorded[1].HoldTime.Ticks, Is.EqualTo(55));
            Assert.That(testFormattter.DataRecorded[1].FlightTime.Ticks, Is.EqualTo(-40));
        }


        /*
         * Test list
         *   
         * Flight time
         * 
         *  
         * Digraph
         *  pressing a key and pressing another should record the Di1
         *  releasing a key and releasing another should record the Di2
         *  pressing a key and releasing another should record the Di3
         */

        #region helper functions
        private void PressKey(char charPressed)
        {
            testFormattter.KeyWasPressed(charPressed, timeElapsed);
        }

        private void Wait(long timeToWait)
        {
            timeElapsed += timeToWait;
            timerFake.AddToElapsed(timeToWait, testFormattter);
        }

        private void ReleaseKey(char charReleased)
        {
            testFormattter.KeyWasReleased(charReleased, timeElapsed);
        }

        private long SecondsToTicks(int seconds)
        {
            return seconds * TimeSpan.TicksPerSecond;
        }
        #endregion
    }
}
