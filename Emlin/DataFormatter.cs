﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Emlin
{
    public class DataFormatter
    {
        private char previousKey;
        private long timeOfPreviousRelease;
        private long timeOfPreviousPress;
        private ITimerInterface timer;
        public enum SessionState { Active , Inactive };

        public List<KeysData> DataRecorded { get; set; } = new List<KeysData>();
        public SessionState CurrentState { get; private set; } = SessionState.Inactive;

        private Dictionary<char, long> keysCurrentlyHeld = new Dictionary<char, long>();
        

        public DataFormatter(ITimerInterface timer)
        {
            this.timer = timer;    
        }

        public void KeyWasPressed(char charPressed, long timeInTicks)
        {
            Console.WriteLine(charPressed.ToString() + " pressed at " + (timeInTicks/TimeSpan.TicksPerMillisecond).ToString());

            if (keysCurrentlyHeld.ContainsKey(charPressed))
            {
                return;
            }

            keysCurrentlyHeld.Add(charPressed, timeInTicks);

            ResetTimer();

            if (CurrentState.Equals(SessionState.Inactive))
            {
                CurrentState = SessionState.Active;
            }
            else
            {
                DataRecorded.Last().CombinationID = HelperFunctions.GetCombinationId(previousKey, charPressed);
                DataRecorded.Last().SecondChar = charPressed;
                DataRecorded.Last().FlightTime = new TimeSpan(timeInTicks - timeOfPreviousRelease);            
            }

            DataRecorded.Add(
                new KeysData
                {
                    FirstChar = charPressed
                });

            previousKey = charPressed;
            timeOfPreviousPress = timeInTicks;
        }


        public void KeyWasReleased(char charReleased, long timeInTicks)
        {
            Console.WriteLine(charReleased.ToString() + " released at " + (timeInTicks / TimeSpan.TicksPerMillisecond).ToString());


            KeysData keysData = GetCorrectKeysData(charReleased);

            RecordOnReleaseData(charReleased, timeInTicks, keysData);

            keysCurrentlyHeld.Remove(charReleased);
            timeOfPreviousRelease = timeInTicks;
        }

        private void RecordOnReleaseData(char charReleased, long timeInTicks, KeysData keysData)
        {
            keysData.HoldTime = new TimeSpan(timeInTicks - keysCurrentlyHeld[charReleased]);

            if (NextKeyAlreadyPressed(keysData))
            {
                keysData.FlightTime = new TimeSpan(keysData.FlightTime.Ticks - timeInTicks);
            }
        }

        private bool NextKeyAlreadyPressed(KeysData keysData)
        {
            return previousKey != keysData.FirstChar;
        }

        private KeysData GetCorrectKeysData(char character)
        {
            return DataRecorded.Last(x => x.FirstChar == character);       
        }

        public void End()
        {
            CurrentState = SessionState.Inactive;
            timer.Enabled = false;
        }

        private void ResetTimer()
        {
            timer.Stop();
            timer.Start();
        }
    }
}