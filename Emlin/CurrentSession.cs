﻿using System;
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



                foreach(KeyCombination keyComb in keysPressed)
                {
                    
                    if (keyComb.CombId != combId)
                    {
                        continue;
                    }
                    
                }
                KeysPressed.Add(new KeyCombination(combId));

                long difference = timeInTicks - previousTime;
                KeysPressed[combId].TimeSpanList.Add(new TimeSpan(difference));
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
