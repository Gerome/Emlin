using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Emlin
{
    public class DataFormatter
    {
        private char lastKeyPress;
        private long timeOfPreviousRelease;
        private long timeOfPreviousPress;
        KeysData previousKeysData;

        private ITimerInterface timer;
        public enum SessionState { Active , Inactive };

        public List<KeysData> DataRecorded { get; private set; } = new List<KeysData>();
        public SessionState CurrentState { get; private set; } = SessionState.Inactive;

        private Dictionary<char, long> keysCurrentlyHeld = new Dictionary<char, long>();
        

        public DataFormatter(ITimerInterface timer)
        {
            this.timer = timer;    
        }

        public void KeyWasPressed(char charPressed, long timeInTicks)
        {
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
                DataRecorded.Last().CombinationID = HelperFunctions.GetCombinationId(lastKeyPress, charPressed);
                DataRecorded.Last().SecondChar = charPressed;
                DataRecorded.Last().FlightTime = new TimeSpan(timeInTicks - timeOfPreviousRelease);            
                DataRecorded.Last().Digraph1 = new TimeSpan(timeInTicks - timeOfPreviousPress);
            }

            DataRecorded.Add(
                new KeysData
                {
                    FirstChar = charPressed
                });

            timeOfPreviousPress = timeInTicks;
            lastKeyPress = charPressed;          
        }


        public void KeyWasReleased(char charReleased, long timeInTicks)
        {
            KeysData keysData = GetCorrectKeysData(charReleased);

            RecordOnReleaseData(charReleased, timeInTicks, keysData);

            keysCurrentlyHeld.Remove(charReleased);
            timeOfPreviousRelease = timeInTicks;
            previousKeysData = keysData;
        }

        private void RecordOnReleaseData(char charReleased, long timeInTicks, KeysData keysData)
        {
            keysData.HoldTime = new TimeSpan(timeInTicks - keysCurrentlyHeld[charReleased]);

            if (NextKeyAlreadyPressed(keysData))
            {
                keysData.FlightTime = new TimeSpan(keysData.FlightTime.Ticks - timeInTicks);
            }

            if (previousKeysData != null)
            {
                if(previousKeysData.FirstChar == keysData.SecondChar) // This happens when the second key of the key combination is released first
                {
                    keysData.Digraph2 = new TimeSpan(timeOfPreviousRelease - timeInTicks);
                    keysData.Digraph3 = keysData.HoldTime + keysData.Digraph2;
                }
                else
                {
                    previousKeysData.Digraph2 = new TimeSpan(timeInTicks - timeOfPreviousRelease);
                    previousKeysData.Digraph3 = previousKeysData.HoldTime + previousKeysData.Digraph2;
                }
            }

        }

        private bool NextKeyAlreadyPressed(KeysData keysData)
        {
            return lastKeyPress != keysData.FirstChar;
        }

        private KeysData GetCorrectKeysData(char character)
        {
            return DataRecorded.Last(x => x.FirstChar == character);       
        }

        public void End()
        {
            CurrentState = SessionState.Inactive;
            timer.Enabled = false;
            DataRecorded = new List<KeysData>();
        }

        public void RemoveLastDataItem()
        {     
            DataRecorded.Remove(DataRecorded.Last());   
        }

        private void ResetTimer()
        {
            timer.Stop();
            timer.Start();
        }
    }
}
