
using System;
using System.Timers;

namespace Emlin
{
    public class CurrentSession
    {
        private KeyCombination[] keysPressed = new KeyCombination[96 * 96];
        private char previousKey;
        private long previousTime;
        
        private SessionState currentState = SessionState.Inactive;
        public SessionState CurrentState { get => currentState; }
        public enum SessionState { Active , Inactive };
        public KeyCombination[] KeysPressed { get => keysPressed; set => keysPressed = value; }

        private ITimerInterface timer;

        public CurrentSession(ITimerInterface timer)
        {
            this.timer = timer;
            timer.Elapsed += TimerCountdown;     
        }

        public void KeyWasPressed(char keyChar, long timeInTicks)
        {
            timer.Start();
            if (CurrentState.Equals(SessionState.Inactive))
            {
                previousKey = keyChar;
                previousTime = timeInTicks;
                currentState = SessionState.Active;
            }
            else
            {
                int combId = HelperFunctions.GetCombinationId(previousKey, keyChar);

  
                if (KeysPressed[combId] == null)
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
            timer.Enabled = false;
        }

        private void TimerCountdown(object sender, ElapsedEventArgs e)
        {
            TimeToFileRecorder ttfRec = new TimeToFileRecorder();
            ttfRec.WriteRecordedDataToFile(keysPressed, ConstantValues.keyboardDataFilepath);
            EndSession();
        }
    }
}
