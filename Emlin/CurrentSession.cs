using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Emlin
{
    public class CurrentSession
    {
        private List<KeyCombination> keysPressed = new List<KeyCombination>();
        private char previousKey;
        private long previousTime;
        
        private SessionState currentState = SessionState.Inactive;
        public SessionState CurrentState { get => currentState; }
        public enum SessionState { Active , Inactive };

        public List<KeyCombination> KeysPressed { get => keysPressed; set => keysPressed = value; }

        private ITimerInterface timer;

        public CurrentSession(ITimerInterface timer)
        {
            this.timer = timer;    
        }

        public void KeyWasPressed(char keyChar, long timeInTicks)
        {
            ResetTimer();     

            if (CurrentState.Equals(SessionState.Inactive))
            {
                currentState = SessionState.Active;
                
            }
            else
            {
                int combId = HelperFunctions.GetCombinationId(previousKey, keyChar);


                long difference = timeInTicks - previousTime;

                foreach(KeyCombination keyComb in keysPressed)
                {
                    
                    if (keyComb.CombId == combId)
                    {
                        keyComb.AddTimespanToList(new TimeSpan(difference));
                        previousKey = keyChar;
                        previousTime = timeInTicks;
                        return;              
                    }
                }

                KeyCombination keyCombToAdd = new KeyCombination(combId);
                keyCombToAdd.AddTimespanToList(new TimeSpan(difference));
                KeysPressed.Add(keyCombToAdd);

            }

            previousKey = keyChar;
            previousTime = timeInTicks;
        }

        public void End()
        {
            currentState = SessionState.Inactive;
            ResetKeysPressedList();
            timer.Enabled = false;
        }

        private void ResetTimer()
        {
            timer.Stop();
            timer.Start();
        }

        private void ResetKeysPressedList()
        {
            keysPressed = new List<KeyCombination>();
        }

    }
}
