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
        KeysData lastKeyReleased;

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
                DataRecorded.Last().Digraph1 = new TimeSpan(timeInTicks - timeOfPreviousPress);
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
            KeysData keysData = GetCorrectKeysData(charReleased);

            RecordOnReleaseData(charReleased, timeInTicks, keysData);

            keysCurrentlyHeld.Remove(charReleased);
            timeOfPreviousRelease = timeInTicks;
            lastKeyReleased = keysData;
        }

        private void RecordOnReleaseData(char charReleased, long timeInTicks, KeysData keysData)
        {
            keysData.HoldTime = new TimeSpan(timeInTicks - keysCurrentlyHeld[charReleased]);

            if (NextKeyAlreadyPressed(keysData))
            {
                keysData.FlightTime = new TimeSpan(keysData.FlightTime.Ticks - timeInTicks);
            }

            if (lastKeyReleased != null)
            {
                if(lastKeyReleased.FirstChar == keysData.SecondChar)
                {
                    keysData.Digraph2 = new TimeSpan(timeOfPreviousRelease - timeInTicks);
                }
                else
                {
                    lastKeyReleased.Digraph2 = new TimeSpan(timeInTicks - timeOfPreviousRelease);
                }
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
            DataRecorded = new List<KeysData>();
        }

        private void ResetTimer()
        {
            timer.Stop();
            timer.Start();
        }
    }
}
