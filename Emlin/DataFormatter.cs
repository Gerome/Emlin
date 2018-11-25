using System;
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

        public void KeyWasPressed(char keyChar, long timeInTicks)
        {
            if (keysCurrentlyHeld.ContainsKey(keyChar))
            {
                return;
            }

            keysCurrentlyHeld.Add(keyChar, timeInTicks);

            ResetTimer();

            if (CurrentState.Equals(SessionState.Inactive))
            {
                CurrentState = SessionState.Active;
            }
            else
            {
                DataRecorded.Last().CombinationID = HelperFunctions.GetCombinationId(previousKey, keyChar);
                DataRecorded.Last().SecondChar = keyChar;
                DataRecorded.Last().FlightTime = new TimeSpan(timeInTicks - timeOfPreviousRelease);            
            }

            DataRecorded.Add(
                new KeysData
                {
                    FirstChar = keyChar
                });

            previousKey = keyChar;
            timeOfPreviousPress = timeInTicks;
        }


        public void KeyWasReleased(char charReleased, long timeInTicks)
        {
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
            //ResetKeysPressedList();
            timer.Enabled = false;
        }

        private void ResetTimer()
        {
            timer.Stop();
            timer.Start();
        }

        //private void ResetKeysPressedList()
        //{
        //    KeysPressed = new List<KeyCombination>();
        //}
    }
}
