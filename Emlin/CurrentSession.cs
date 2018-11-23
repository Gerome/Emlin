using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Emlin
{
    public class CurrentSession
    {
        private char previousKey;
        private long previousTime;
        private ITimerInterface timer;
        public enum SessionState { Active , Inactive };

        public List<KeysData> DataRecorded { get; set; } = new List<KeysData>();
        public SessionState CurrentState { get; private set; } = SessionState.Inactive;
        

        public CurrentSession(ITimerInterface timer)
        {
            this.timer = timer;    
        }

        public void KeyWasPressed(char keyChar, long timeInTicks)
        {
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

            //else
            //{     
                //    int combId = HelperFunctions.GetCombinationId(previousKey, keyChar);


                //    long difference = timeInTicks - previousTime;

                //    foreach(KeyCombination keyComb in KeysPressed)
                //    {

                //        if (keyComb.CombId == combId)
                //        {
                //            keyComb.AddTimespanToList(new TimeSpan(difference));
                //            previousKey = keyChar;
                //            previousTime = timeInTicks;
                //            return;              
                //        }
                //    }

                //    KeyCombination keyCombToAdd = new KeyCombination(combId);
                //    keyCombToAdd.AddTimespanToList(new TimeSpan(difference));
                //    KeysPressed.Add(keyCombToAdd);
            //}

            previousKey = keyChar;
            previousTime = timeInTicks;
        }

        public void KeyWasReleased(char charReleased, long timeInTicks)
        {
            DataRecorded.Last().HoldTime = new TimeSpan(timeInTicks - previousTime);
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
