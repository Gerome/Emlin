using Emlin;
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
        public void Sess_Current_session_is_inactive_by_default()
        {
            Assert.That(testFormattter.CurrentState, Is.EqualTo(DataFormatter.SessionState.Inactive));
        }

        [Test]
        public void Sess_Pressing_a_key_sets_the_state_of_the_session_to_active()
        {
            PressKey('A');
            Assert.That(testFormattter.CurrentState, Is.EqualTo(DataFormatter.SessionState.Active));
        }

        [Test]
        public void Sess_Pressing_2_key_sets_the_state_of_the_session_to_active()
        {
            PressKey('A');
            Wait(200);
            PressKey('B');
            Assert.That(testFormattter.CurrentState, Is.EqualTo(DataFormatter.SessionState.Active));
        }

        [Test]
        public void Sess_Pressing_3_key_with_1_second_delay_keeps_session_active()
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
        public void Sess_Pressing_a_key_and_waiting_2_seconds_shows_the_session_is_inactive()
        {
            PressKey('A');
            Assert.That(testFormattter.CurrentState, Is.EqualTo(DataFormatter.SessionState.Active));
            Wait(SecondsToTicks(2));
            Assert.That(testFormattter.CurrentState, Is.EqualTo(DataFormatter.SessionState.Inactive));
        }

        [Test]
        public void Sess_Holding_a_key_and_waiting_2_seconds_shows_the_session_is_still_active()
        {
            PressKey('A');
            Wait(SecondsToTicks(1));
            PressKey('A');
            Wait(SecondsToTicks(1));
            Assert.That(testFormattter.CurrentState, Is.EqualTo(DataFormatter.SessionState.Active));
        }

        [Test]
        public void Ht_Pressing_and_Releasing_a_key_should_record_the_Hold_Time()
        {
            PressRelease_A();

            Assert.That(testFormattter.DataRecorded.First().HoldTime.Ticks, Is.EqualTo(100));
        }


        [Test]
        public void Ht_Pressing_and_Releasing_2_keys_should_record_the_Hold_Time_of_each()
        {
            PressRelease_A_and_B();

            Assert.That(testFormattter.DataRecorded.First().HoldTime.Ticks, Is.EqualTo(100));
            Assert.That(testFormattter.DataRecorded[1].HoldTime.Ticks, Is.EqualTo(200));
        }

       

        [Test]
        public void Ht_Pressing_and_Releasing_the_same_2_keys_should_record_the_Hold_Time_of_each()
        {
            PressRelease_A();
            PressRelease_A();

            Assert.That(testFormattter.DataRecorded.First().HoldTime.Ticks, Is.EqualTo(100));
            Assert.That(testFormattter.DataRecorded[1].HoldTime.Ticks, Is.EqualTo(100));
        }

        [Test]
        public void Ht_Pressing_and_Releasing_2_keys_intermediately_should_still_record_the_Hold_Time_of_each()
        {
            Press_A_B_Release_A_B();

            Assert.That(testFormattter.DataRecorded.First().HoldTime.Ticks, Is.EqualTo(250));
            Assert.That(testFormattter.DataRecorded[1].HoldTime.Ticks, Is.EqualTo(300));
        }

        [Test]
        public void Crash_Holding_down_a_key_should_not_crash_the_program()
        {
            PressKey('a');
            PressKey('a');
            PressKey('a');
            PressKey('a');
            PressKey('a');
            PressKey('a');
        }

        [Test]
        public void Crash_Holding_down_multiple_keys_should_not_crash_the_program()
        {
            PressKey('a');
            PressKey('b');
            PressKey('a');
            PressKey('b');
        }

        [Test]
        public void Ht_Holding_down_a_key_should_record_the_hold_time()
        {
            Press_A();
            Press_A();
            Press_A();
            Assert.That(testFormattter.KeysPressedAndReleased.Count, Is.EqualTo(1));
            Release_A();
            Assert.That(testFormattter.KeysPressedAndReleased.Count, Is.EqualTo(1));

            Assert.That(testFormattter.DataRecorded.First().HoldTime.Ticks, Is.EqualTo(200));
        }

        [Test]
        public void Id_Pressing_a_key_and_another_should_record_the_combination_ID()
        {
            PressKey('a');
            PressKey('b');
            int combId = HelperFunctions.GetCombinationId('a', 'b');

            Assert.That(testFormattter.DataRecorded.First().CombinationID, Is.EqualTo(combId));
        }

        [Test]
        public void Id_Pressing_3_keys_should_record_the_combination_IDs()
        {
            PressKey('a');
            PressKey('b');
            PressKey('c');
            int combId = HelperFunctions.GetCombinationId('a', 'b');
            Assert.That(testFormattter.DataRecorded.First().CombinationID, Is.EqualTo(combId));

            combId = HelperFunctions.GetCombinationId('b', 'c');
            Assert.That(testFormattter.DataRecorded[1].CombinationID, Is.EqualTo(combId));
        }

        [Test]
        public void Ft_Releasing_a_key_and_pressing_another_should_record_the_flight_time()
        {
            PressRelease_A();
            Wait(50);
            PressKey('b');

            Assert.That(testFormattter.DataRecorded.First().FlightTime.Ticks, Is.EqualTo(50));
        }

        [Test]
        public void Ft_Pressing_two_keys_then_releasing_one_should_record_a_negative_flight_time()
        {
            Press_A_B_Release_A_B();

            Assert.That(testFormattter.DataRecorded.First().FlightTime.Ticks, Is.EqualTo(-100));
        }

        [Test]
        public void Ft_Pressing_two_keys_then_releasing_the_second_first_should_record_a_negative_flight_time()
        {
            Press_A_B_Release_B_A();

            Assert.That(testFormattter.DataRecorded.First().FlightTime.Ticks, Is.EqualTo(-300));
        }

        [Test]
        public void Ft_should_record_the_flight_time_correctly()
        {
            PressKey('a');
            Wait(10);
            PressKey('b');
            Wait(15);
            ReleaseKey('a');
            Wait(20);
            PressKey('c');
            Wait(25);
            ReleaseKey('b');
            Wait(30);
            ReleaseKey('c');

            Assert.That(testFormattter.DataRecorded.First().FlightTime.Ticks, Is.EqualTo(-15));
            Assert.That(testFormattter.DataRecorded[1].FlightTime.Ticks, Is.EqualTo(-25));
        }

        [Test]
        public void Ht_Ft_Holding_three_keys_and_releasing_in_order_record_Hold_and_Flight_time_correctly()
        {
            Wait(10);
            PressKey('a');   // 10
            Wait(10);
            PressKey('b');   // 20
            Wait(15);
            PressKey('c');   // 35
            Wait(15);
            ReleaseKey('a'); // 50
            Wait(25);
            ReleaseKey('b'); // 75
            Wait(30);
            ReleaseKey('c'); // 105

            Assert.That(testFormattter.DataRecorded.First().HoldTime.Ticks, Is.EqualTo(40));
            Assert.That(testFormattter.DataRecorded.First().FlightTime.Ticks, Is.EqualTo(-30));

            Assert.That(testFormattter.DataRecorded[1].HoldTime.Ticks, Is.EqualTo(55));
            Assert.That(testFormattter.DataRecorded[1].FlightTime.Ticks, Is.EqualTo(-40));
        }

        [Test]
        public void Ht_Ft_Holding_three_keys_and_releasing_not_in_order_record_Hold_and_Flight_time_correctly()
        {
            Wait(10);      
            PressKey('a');   // 10
            Wait(10);
            PressKey('b');   // 20
            Wait(15);
            PressKey('c');   // 35
            Wait(15);
            ReleaseKey('c'); // 50
            Wait(25);
            ReleaseKey('b'); // 75
            Wait(30);
            ReleaseKey('a'); // 105

            Assert.That(testFormattter.DataRecorded.First().HoldTime.Ticks, Is.EqualTo(95));
            Assert.That(testFormattter.DataRecorded.First().FlightTime.Ticks, Is.EqualTo(-85));

            Assert.That(testFormattter.DataRecorded[1].HoldTime.Ticks, Is.EqualTo(55));
            Assert.That(testFormattter.DataRecorded[1].FlightTime.Ticks, Is.EqualTo(-40));
        }

        [Test]
        public void Data_recorded_is_cleared_once_the_timer_counts_down()
        {
            PressRelease_A();
            PressKey('b');

            Wait(SecondsToTicks(5));
            Assert.That(testFormattter.DataRecorded, Is.Empty);
        }

        // Impossible to have a negative Digraph1
        [Test]
        public void Di1_Pressing_a_key_and_pressing_another_should_record_the_Digraph1()
        {
            PressRelease_A();
            PressKey('b');

            Assert.That(testFormattter.DataRecorded.First().Digraph1.Ticks, Is.EqualTo(100));
        }

        [Test]
        public void Di1_Holding_three_keys_and_releasing_in_order_record_Digraph1_time_correctly()
        {
            Wait(10);
            PressKey('a');   // 10
            Wait(10);
            PressKey('b');   // 20
            Wait(15);
            PressKey('c');   // 35

            Assert.That(testFormattter.DataRecorded.First().Digraph1.Ticks, Is.EqualTo(10));
            Assert.That(testFormattter.DataRecorded[1].Digraph1.Ticks, Is.EqualTo(15));
        }

        [Test]
        public void Di2_Pressing_a_key_and_releasing_should_record_the_Digraph2_as_0()
        {
            PressRelease_A();

            Assert.That(testFormattter.DataRecorded.First().Digraph2.Ticks, Is.EqualTo(0));
        }

        [Test]
        public void Di2_Pressing_a_key_and_pressing_another_should_record_the_Digraph2()
        {
            PressRelease_A_and_B();

            Assert.That(testFormattter.DataRecorded.First().Digraph2.Ticks, Is.EqualTo(350)); 
        }

        [Test]
        public void Di2_Pressing_two_keys_and_releasing_the_first_should_record_the_Digraph2()
        {
            Press_A_B_Release_A_B();

            Assert.That(testFormattter.DataRecorded.First().Digraph2.Ticks, Is.EqualTo(200));
        }

        [Test]
        public void Di2_Pressing_two_keys_and_releasing_the_second_first_should_record_a_negative_Digaph2()
        {
            Press_A_B_Release_B_A();

            Assert.That(testFormattter.DataRecorded.First().Digraph2.Ticks, Is.EqualTo(-100));
        }

        [Test]
        public void Di2_Holding_three_keys_and_releasing_in_order_record_Digraph2_time_correctly()
        {
            Wait(10);
            PressKey('a');   // 10
            Wait(10);
            PressKey('b');   // 20
            Wait(15);
            PressKey('c');   // 35
            Wait(15);
            ReleaseKey('a'); // 50
            Wait(25);
            ReleaseKey('b'); // 75
            Wait(30);
            ReleaseKey('c'); // 105

            Assert.That(testFormattter.DataRecorded.First().Digraph2.Ticks, Is.EqualTo(25));
            Assert.That(testFormattter.DataRecorded[1].Digraph2.Ticks, Is.EqualTo(30));
        }

        [Test]
        public void Di2_Holding_three_keys_and_releasing_not_in_order_record_the_Digraph2_correctly()
        {
            Wait(10);
            PressKey('a');   // 10
            Wait(10);
            PressKey('b');   // 20
            Wait(15);
            PressKey('c');   // 35
            Wait(15);
            ReleaseKey('c'); // 50
            Wait(25);
            ReleaseKey('b'); // 75
            Wait(30);
            ReleaseKey('a'); // 105

            Assert.That(testFormattter.DataRecorded.First().Digraph2.Ticks, Is.EqualTo(-30));
            Assert.That(testFormattter.DataRecorded[1].Digraph2.Ticks, Is.EqualTo(-25));
        }

        
        [Test]
        public void Di3_Pressing_and_releasing_two_keys_should_record_the_digraph3_time()
        {
            Press_A_B_Release_A_B();

            Assert.That(testFormattter.DataRecorded.First().Digraph3.Ticks, Is.EqualTo(450));
        }

        [Test]
        public void Di3_Pressing_and_releasing_multiple_two_keys_should_record_the_digraph3_time()
        {
            Press_A_B_Release_A_B();
            Assert.That(testFormattter.KeysPressedAndReleased.Count, Is.EqualTo(1));

            Press_A_B_Release_A_B();
            Assert.That(testFormattter.KeysPressedAndReleased.Count, Is.EqualTo(1));

            Press_A_B_Release_A_B();
            Assert.That(testFormattter.KeysPressedAndReleased.Count, Is.EqualTo(1));


            Assert.That(testFormattter.DataRecorded.First().Digraph3.Ticks, Is.EqualTo(450));
            Assert.That(testFormattter.DataRecorded[2].Digraph3.Ticks, Is.EqualTo(450));
        }


        [Test]
        public void Di3_Pressing_two_keys_and_releasing_the_second_first_should_record_a_the_correct_Digaph3()
        {
            Press_A_B_Release_B_A();

            Assert.That(testFormattter.DataRecorded.First().Digraph3.Ticks, Is.EqualTo(350));
        }

        
        [Test]
        public void Di3_Holding_three_keys_and_releasing_not_in_order_record_the_Digraph3_correctly()
        {
            PressKey('a');   // 0
            Wait(10);
            PressKey('b');   // 10
            Wait(15);
            PressKey('c');   // 25
            Wait(20);
            ReleaseKey('b'); // 45
            Wait(25);
            ReleaseKey('a'); // 70
            Wait(30);
            ReleaseKey('c'); // 100

            Assert.That(testFormattter.DataRecorded.First().Digraph3.Ticks, Is.EqualTo(45));
            Assert.That(testFormattter.DataRecorded[1].Digraph3.Ticks, Is.EqualTo(90));
        }

        [Test]
        public void All_Specific_key_comb_should_record_all_correctly()
        {
            PressKey('a'); // 0
            Wait(10);
            PressKey('b'); // 10
            Wait(15);
            PressKey('c'); // 25
            Wait(20);
            ReleaseKey('b'); // 45
            Wait(25);
            ReleaseKey('c'); // 70
            Wait(30);
            ReleaseKey('a'); // 100

            Assert.That(testFormattter.DataRecorded.First().HoldTime.Ticks, Is.EqualTo(100));
            Assert.That(testFormattter.DataRecorded[1].HoldTime.Ticks, Is.EqualTo(35));

            Assert.That(testFormattter.DataRecorded.First().FlightTime.Ticks, Is.EqualTo(-90));
            Assert.That(testFormattter.DataRecorded[1].FlightTime.Ticks, Is.EqualTo(-20));

            Assert.That(testFormattter.DataRecorded.First().Digraph1.Ticks, Is.EqualTo(10));
            Assert.That(testFormattter.DataRecorded[1].Digraph1.Ticks, Is.EqualTo(15));

            Assert.That(testFormattter.DataRecorded.First().Digraph2.Ticks, Is.EqualTo(-55));
            Assert.That(testFormattter.DataRecorded.First().Digraph3.Ticks, Is.EqualTo(45));

            Assert.That(testFormattter.DataRecorded[1].Digraph2.Ticks, Is.EqualTo(25));
            Assert.That(testFormattter.DataRecorded[1].Digraph3.Ticks, Is.EqualTo(60));
        }


        [Test]
        public void All_test()
        {
            PressKey('a');
            Wait(1);
            PressKey('b');
            Wait(2);
            PressKey('c');
            Wait(3);
            PressKey('d');
            Wait(4);

            ReleaseKey('d');
            Wait(5);
            ReleaseKey('a');
            Wait(6);
            ReleaseKey('b');
            Wait(7);
            ReleaseKey('c');
            Wait(8);

            Assert.That(testFormattter.DataRecorded.First().FlightTime.Ticks, Is.EqualTo(-14));
            Assert.That(testFormattter.DataRecorded.First().HoldTime.Ticks, Is.EqualTo(15));
            Assert.That(testFormattter.DataRecorded.First().Digraph1.Ticks, Is.EqualTo(1));
            Assert.That(testFormattter.DataRecorded.First().Digraph2.Ticks, Is.EqualTo(6));
            Assert.That(testFormattter.DataRecorded.First().Digraph3.Ticks, Is.EqualTo(21));

            Assert.That(testFormattter.DataRecorded[1].FlightTime.Ticks, Is.EqualTo(-18));
            Assert.That(testFormattter.DataRecorded[1].HoldTime.Ticks, Is.EqualTo(20));
            Assert.That(testFormattter.DataRecorded[1].Digraph1.Ticks, Is.EqualTo(2));
            Assert.That(testFormattter.DataRecorded[1].Digraph2.Ticks, Is.EqualTo(7));
            Assert.That(testFormattter.DataRecorded[1].Digraph3.Ticks, Is.EqualTo(27));

            Assert.That(testFormattter.DataRecorded[2].FlightTime.Ticks, Is.EqualTo(-22));
            Assert.That(testFormattter.DataRecorded[2].HoldTime.Ticks, Is.EqualTo(25));
            Assert.That(testFormattter.DataRecorded[2].Digraph1.Ticks, Is.EqualTo(3));
            Assert.That(testFormattter.DataRecorded[2].Digraph2.Ticks, Is.EqualTo(-18));
            Assert.That(testFormattter.DataRecorded[2].Digraph3.Ticks, Is.EqualTo(7));

           

            PressKey('d');
            Wait(9);
            PressKey('a');
            Wait(1);
            PressKey('b');
            Wait(2);
            PressKey('c');
            Wait(3);

            ReleaseKey('b');
            Wait(4);
            ReleaseKey('d');
            Wait(5);
            ReleaseKey('c');
            Wait(6);
            ReleaseKey('a');
            Wait(7);

            Assert.That(testFormattter.DataRecorded[3].FlightTime.Ticks, Is.EqualTo(26));
            Assert.That(testFormattter.DataRecorded[3].HoldTime.Ticks, Is.EqualTo(4));
            Assert.That(testFormattter.DataRecorded[3].Digraph1.Ticks, Is.EqualTo(30));
            Assert.That(testFormattter.DataRecorded[3].Digraph2.Ticks, Is.EqualTo(45));
            Assert.That(testFormattter.DataRecorded[3].Digraph3.Ticks, Is.EqualTo(49));

            Assert.That(testFormattter.DataRecorded[4].FlightTime.Ticks, Is.EqualTo(-10));
            Assert.That(testFormattter.DataRecorded[4].HoldTime.Ticks, Is.EqualTo(19));
            Assert.That(testFormattter.DataRecorded[4].Digraph1.Ticks, Is.EqualTo(9));
            Assert.That(testFormattter.DataRecorded[4].Digraph2.Ticks, Is.EqualTo(11));
            Assert.That(testFormattter.DataRecorded[4].Digraph3.Ticks, Is.EqualTo(30));

            Assert.That(testFormattter.DataRecorded[5].FlightTime.Ticks, Is.EqualTo(-20));
            Assert.That(testFormattter.DataRecorded[5].HoldTime.Ticks, Is.EqualTo(21));
            Assert.That(testFormattter.DataRecorded[5].Digraph1.Ticks, Is.EqualTo(1));
            Assert.That(testFormattter.DataRecorded[5].Digraph2.Ticks, Is.EqualTo(-15));
            Assert.That(testFormattter.DataRecorded[5].Digraph3.Ticks, Is.EqualTo(6));

            Assert.That(testFormattter.DataRecorded[6].FlightTime.Ticks, Is.EqualTo(-3));
            Assert.That(testFormattter.DataRecorded[6].HoldTime.Ticks, Is.EqualTo(5));
            Assert.That(testFormattter.DataRecorded[6].Digraph1.Ticks, Is.EqualTo(2));
            Assert.That(testFormattter.DataRecorded[6].Digraph2.Ticks, Is.EqualTo(9));
            Assert.That(testFormattter.DataRecorded[6].Digraph3.Ticks, Is.EqualTo(14));

            Assert.That(testFormattter.DataRecorded[7].FlightTime.Ticks, Is.EqualTo(0));
            Assert.That(testFormattter.DataRecorded[7].HoldTime.Ticks, Is.EqualTo(12));
            Assert.That(testFormattter.DataRecorded[7].Digraph1.Ticks, Is.EqualTo(0));
            Assert.That(testFormattter.DataRecorded[7].Digraph2.Ticks, Is.EqualTo(0));
            Assert.That(testFormattter.DataRecorded[7].Digraph3.Ticks, Is.EqualTo(0));
        }

        [Test]
        public void Di2_Di3_Intermitent_chars_of_same_char()
        {
            PressKey('a');
            Assert.That(testFormattter.KeysPressedAndReleased.Count, Is.EqualTo(1));

            Wait(10);
            PressKey('b');
            Assert.That(testFormattter.KeysPressedAndReleased.Count, Is.EqualTo(2));

            Wait(15);
            ReleaseKey('a');
            Assert.That(testFormattter.KeysPressedAndReleased.Count, Is.EqualTo(2));

            Wait(20);
            PressKey('a');
            Assert.That(testFormattter.KeysPressedAndReleased.Count, Is.EqualTo(3));

            Wait(25);
            ReleaseKey('b');
            Assert.That(testFormattter.KeysPressedAndReleased.Count, Is.EqualTo(2));

            Wait(30);
            ReleaseKey('a');
            Assert.That(testFormattter.KeysPressedAndReleased.Count, Is.EqualTo(1));



            Assert.That(testFormattter.DataRecorded[1].Digraph2.Ticks, Is.EqualTo(30));
            Assert.That(testFormattter.DataRecorded[1].Digraph3.Ticks, Is.EqualTo(90)); 
            Assert.That(testFormattter.DataRecorded.First().Digraph2.Ticks, Is.EqualTo(45));
            Assert.That(testFormattter.DataRecorded.First().Digraph3.Ticks, Is.EqualTo(70));
        }

        [Test]
        public void Di2_Di3_Intermitent_chars_of_same_char_inbetween()
        {
            PressKey('a');
            Assert.That(testFormattter.KeysPressedAndReleased.Count, Is.EqualTo(1));
            Wait(10);
            PressKey('b');
            Assert.That(testFormattter.KeysPressedAndReleased.Count, Is.EqualTo(2));
            Assert.That(testFormattter.KeysPressedAndReleased[1].TimePressedInTicks, Is.EqualTo(10));
            Wait(15);
            ReleaseKey('b');
            Assert.That(testFormattter.KeysPressedAndReleased.Count, Is.EqualTo(2));
            Assert.That(testFormattter.KeysPressedAndReleased[1].TimeReleasedInTicks, Is.EqualTo(25));
            Wait(20);
            PressKey('b');
            Assert.That(testFormattter.KeysPressedAndReleased.Count, Is.EqualTo(3));
            Assert.That(testFormattter.KeysPressedAndReleased[2].TimePressedInTicks, Is.EqualTo(45));
            Wait(25);
            ReleaseKey('b');
            Assert.That(testFormattter.KeysPressedAndReleased.Count, Is.EqualTo(3));
            Assert.That(testFormattter.KeysPressedAndReleased[2].TimeReleasedInTicks, Is.EqualTo(70));
            PressKey('b');
            Assert.That(testFormattter.KeysPressedAndReleased.Count, Is.EqualTo(4));
            Assert.That(testFormattter.KeysPressedAndReleased[3].TimePressedInTicks, Is.EqualTo(70));
            Wait(25);
            ReleaseKey('b');
            Assert.That(testFormattter.KeysPressedAndReleased.Count, Is.EqualTo(3));
            //Assert.That(testFormattter.keysPressedAndReleased[2].TimeReleasedInTicks, Is.EqualTo(95));
            Wait(30);
            ReleaseKey('a');
            Assert.That(testFormattter.KeysPressedAndReleased.Count, Is.EqualTo(1));

            Assert.That(testFormattter.DataRecorded.First().FlightTime.Ticks, Is.EqualTo(-115));
            Assert.That(testFormattter.DataRecorded.First().HoldTime.Ticks, Is.EqualTo(125));
            Assert.That(testFormattter.DataRecorded.First().Digraph1.Ticks, Is.EqualTo(10));
            Assert.That(testFormattter.DataRecorded.First().Digraph2.Ticks, Is.EqualTo(-100));
            Assert.That(testFormattter.DataRecorded.First().Digraph3.Ticks, Is.EqualTo(25));

            Assert.That(testFormattter.DataRecorded[1].HoldTime.Ticks, Is.EqualTo(15));
            Assert.That(testFormattter.DataRecorded[1].FlightTime.Ticks, Is.EqualTo(20));
            Assert.That(testFormattter.DataRecorded[1].Digraph1.Ticks, Is.EqualTo(35));
            Assert.That(testFormattter.DataRecorded[1].Digraph2.Ticks, Is.EqualTo(45));
            Assert.That(testFormattter.DataRecorded[1].Digraph3.Ticks, Is.EqualTo(60));

        }

        [Test]
        public void Di2_Di3_Same_char_multiple_times()
        {
            PressKey('h');
            Wait(10);
            ReleaseKey('h');
            Wait(10);
            PressKey('h');
            Assert.That(testFormattter.KeysPressedAndReleased.Count, Is.EqualTo(2));
            Wait(5);
            ReleaseKey('h');
            Assert.That(testFormattter.KeysPressedAndReleased.Count, Is.EqualTo(1));
            Assert.That(testFormattter.DataRecorded.First().Digraph2.Ticks, Is.EqualTo(15));
            Assert.That(testFormattter.DataRecorded.First().Digraph3.Ticks, Is.EqualTo(25));
        }


        /*
         * Test list
         *   
         *  pressing a key and releasing another should record the Di3
         */

        [Test]
        public void Remover_Should_remove_the_a_and_b()
        {
            PressKey('a');
            PressKey('b');
            PressKey('c');
            Assert.That(testFormattter.KeysPressedAndReleased.Count, Is.EqualTo(3));
            ReleaseKey('a');
            ReleaseKey('b');
            Assert.That(testFormattter.KeysPressedAndReleased.Count, Is.EqualTo(2));
            ReleaseKey('c');
            Assert.That(testFormattter.KeysPressedAndReleased.Count, Is.EqualTo(1));
        }

        [Test]
        public void Remover_Should_remove_the_a_and_b_ABCCBA()
        {
            PressKey('a');
            PressKey('b');
            PressKey('c');
            Assert.That(testFormattter.KeysPressedAndReleased.Count, Is.EqualTo(3));
            ReleaseKey('c');
            ReleaseKey('b');
            ReleaseKey('a');
            Assert.That(testFormattter.KeysPressedAndReleased.Count, Is.EqualTo(1));
        }

        [Test]
        public void Remover_Should_remove_the_correct_instance_of_C()
        {
            PressKey(' ');
            Wait(10);
            PressKey('u');
            Wait(10);
            ReleaseKey('u');
            Wait(10);
            PressKey('c');
            Wait(10);
            PressKey('l');
            Wait(10);
            ReleaseKey('c');
            Wait(10);
            ReleaseKey('l');
        }

        #region helper functions

        private void PressRelease_A_and_B()
        {
            // 0ms
            PressRelease_A();
            // 150ms
            PressRelease_B();
            // 500ms
        }

        private void Press_A_B_Release_A_B()
        {
            // 0ms
            Press_A();
            // 50ms
            Press_B();
            // 200ms
            Release_A();
            // 300ms
            Release_B();
            // 500ms
        }

        private void Press_A_B_Release_B_A()
        {
            // 0ms
            Press_A();
            // 50ms
            Press_B();
            // 200ms
            Release_B();
            // 400ms
            Release_A();
            // 500ms
        }

        private void PressRelease_A()
        {
            Press_A(); // 50
            Release_A(); // 150
        }
        #region Press Release
        private void Press_A()
        {
            Wait(50);
            PressKey('a');
        }

        private void Release_A()
        {
            Wait(100);
            ReleaseKey('a');
        }

        private void PressRelease_B()
        {
            Press_B();
            Release_B();
        }

        private void Press_B()
        {
            Wait(150);
            PressKey('b');
        }

        private void Release_B()
        {
            Wait(200);
            ReleaseKey('b');
        }

        private void PressKey(char charPressed)
        {
            testFormattter.KeyWasPressed(charPressed, timeElapsed);
        }

        private void ReleaseKey(char charReleased)
        {
            testFormattter.KeyWasReleased(charReleased, timeElapsed);
        }
        #endregion

        private void Wait(long timeToWait)
        {
            timeElapsed += timeToWait;
            timerFake.AddToElapsed(timeToWait, testFormattter);
        }

        private long SecondsToTicks(int seconds)
        {
            return seconds * TimeSpan.TicksPerSecond;
        }
        #endregion
    }
}
