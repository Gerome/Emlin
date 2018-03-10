
using System;
using System.Timers;

namespace Emlin
{
    public class CurrentSession
    {
        private KeyCombination[] keysPressed = new KeyCombination[96 * 96];
        private char previousKey;
        private long previousTime;

        private Timer timeAlive = new Timer(1500);
        

        private SessionState currentState = SessionState.Inactive;
        public SessionState CurrentState { get => currentState; }
        public enum SessionState { Active , Inactive };
        public KeyCombination[] KeysPressed { get => keysPressed; set => keysPressed = value; }

        public CurrentSession()
        {
            timeAlive.Elapsed += TimerCountdown;
            
        }

        internal void KeyWasPressed(char keyChar, long timeInTicks)
        {
            timeAlive.Start();
            if (CurrentState.Equals(SessionState.Inactive))
            {
                previousKey = keyChar;
                previousTime = timeInTicks;
                currentState = SessionState.Active;
            }
            else
            {
                int combId = HelperFunctions.GetCombinationId(previousKey, keyChar);

                KeyCombination keyComb = KeysPressed[combId];

                if (keyComb == null)
                {
                    KeysPressed[combId] = new KeyCombination(combId);
                }
                
                long difference = timeInTicks - previousTime;
                KeysPressed[combId].TimeSpanList.Add(new TimeSpan(difference));
                
            }
        }

        public void EndSession()
        {
            currentState = SessionState.Inactive;
            keysPressed = new KeyCombination[96 * 96];
            timeAlive.Enabled = false;
        }

        private void TimerCountdown(object sender, ElapsedEventArgs e)
        {
            EndSession();
        }
    }
}
