using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Emlin
{
    public class DataFormatter
    {
        private char previousKey;
        private ITimerInterface timer;
        public enum SessionState { Active , Inactive };

        public List<KeysData> DataRecorded { get; set; } = new List<KeysData>();
        public SessionState CurrentState { get; private set; } = SessionState.Inactive;

        private Dictionary<char, long> timeKeyPress = new Dictionary<char, long>();
        

        public DataFormatter(ITimerInterface timer)
        {
            this.timer = timer;    
        }

        public void KeyWasPressed(char keyChar, long timeInTicks)
        {
            if (timeKeyPress.ContainsKey(keyChar))
            {
                return;
            }

            timeKeyPress.Add(keyChar, timeInTicks);

            ResetTimer();

            if (CurrentState.Equals(SessionState.Inactive))
            {
                CurrentState = SessionState.Active;       
            }
            else
            {
                DataRecorded.Last().CombinationID = HelperFunctions.GetCombinationId(previousKey, keyChar);
            }

            KeysData keysData = new KeysData
            {
                FirstChar = keyChar
            };

            DataRecorded.Add(keysData);

            previousKey = keyChar;
        }

        public void KeyWasReleased(char charReleased, long timeInTicks)
        {
            KeysData keysData = GetCorrectKeysData(charReleased);      
            keysData.HoldTime = new TimeSpan(timeInTicks - timeKeyPress[charReleased]);
            timeKeyPress.Remove(charReleased);
        }

        private KeysData GetCorrectKeysData(char charReleased)
        {
            if (DataRecorded.Last().FirstChar.Equals(charReleased))
            {
                return DataRecorded.Last();
            }
            else
            {
                return DataRecorded[DataRecorded.Count - 2];
            }
          
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
