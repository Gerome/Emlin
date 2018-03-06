
using System;
using System.Collections.Generic;


namespace Emlin
{
    public class CurrentSession
    {
        private KeyCombination[] keysPressed;

        private static CurrentSession instance;
        private char previousKey = char.MinValue;

        private long previousTime;


        public KeyCombination[] KeysPressed { get => keysPressed; set => keysPressed = value; }

        private CurrentSession()
        {
            keysPressed = new KeyCombination[96 * 96];
        }

        public static CurrentSession Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CurrentSession();
                }
                return instance;
            }
        }


        internal void KeyWasPressed(char keyChar)
        {
            if (previousKey.Equals(char.MinValue))
            {
                previousKey = keyChar;
                previousTime = DateTime.Now.Ticks;
            }
            else
            {
                int combId = HelperFunctions.GetCombinationId(previousKey, keyChar);

                KeyCombination keyComb = KeysPressed[combId];

                if (keyComb == null)
                {
                    KeysPressed[combId] = new KeyCombination(combId);
                }
                
                long difference = DateTime.Now.Ticks - previousTime;
                KeysPressed[combId].TimeSpanList.Add(new TimeSpan(difference));
                     
            }
        }
    }
}
