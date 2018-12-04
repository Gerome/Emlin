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
        KeysData previousReleaseKeysData;

        private ITimerInterface timer;
        public enum SessionState { Active, Inactive };

        public List<KeysData> DataRecorded { get; private set; } = new List<KeysData>();
        public List<KeysData> UnprocessedData { get; private set; } = new List<KeysData>();
        public SessionState CurrentState { get; private set; } = SessionState.Inactive;

        public List<KeyPressRelease> keysPressedAndReleased = new List<KeyPressRelease>();

        private Dictionary<char, long> keysCurrentlyHeld = new Dictionary<char, long>();

        long timeSinceLastAction = 0;


        public DataFormatter(ITimerInterface timer)
        {
            this.timer = timer;
        }

        public void KeyWasPressed(char charPressed, long timeInTicks)
        {
            Console.WriteLine("Wait(" + TimeSpan.FromTicks(timeSinceLastAction).TotalMilliseconds.ToString() + ");");
            Console.WriteLine("PressKey(\'" + charPressed.ToString() + "\');");
            timeSinceLastAction = timeInTicks;
            ResetTimer();

            if (keysCurrentlyHeld.ContainsKey(charPressed))
            {
                return;
            }

            keysCurrentlyHeld.Add(charPressed, timeInTicks);

            keysPressedAndReleased.Add(new KeyPressRelease(charPressed, timeInTicks));


            if (CurrentState.Equals(SessionState.Inactive))
            {
                CurrentState = SessionState.Active;
            }
            else
            {
                DataRecorded.Last().SecondChar = charPressed;

                DataRecorded.Last().CombinationID = HelperFunctions.GetCombinationId(lastKeyPress, charPressed);


                if (!NextKeyAlreadyPressed(DataRecorded.Last()))
                {
                    DataRecorded.Last().FlightTime = TimeSpan.FromTicks(timeInTicks - timeOfPreviousRelease);
                }

                DataRecorded.Last().Digraph1 = TimeSpan.FromTicks(timeInTicks - timeOfPreviousPress);

                DataRecorded.Last().WaitingForEvents.Remove(KeysData.WaitingForEvent.ESecondKeyPress);
            }

            KeysData keysData = new KeysData
            {
                FirstChar = charPressed
            };

            DataRecorded.Add(keysData);
                

            UnprocessedData.Add(keysData);

            timeOfPreviousPress = timeInTicks;
            lastKeyPress = charPressed;
        }

        List<char> charsReleased = new List<char>();
        public void KeyWasReleased(char charReleased, long timeInTicks)
        {
            charsReleased.Add(charReleased);

            Console.WriteLine("Wait(" + TimeSpan.FromTicks(timeSinceLastAction).TotalMilliseconds.ToString() + ");");
            Console.WriteLine("ReleaseKey(\'" + charReleased.ToString() + "\');");
            timeSinceLastAction = timeInTicks;

            keysPressedAndReleased.Last(x => x.Character == charReleased).TimeReleasedInTicks = timeInTicks;

            KeysData keysData = KeysDataWhereFirstCharIs(charReleased);

            RecordOnReleaseData(charReleased, timeInTicks, keysData);

            keysCurrentlyHeld.Remove(charReleased);
            timeOfPreviousRelease = timeInTicks;
            previousReleaseKeysData = keysData;

        }

        private void RemoveUnnecesarryKeys(char charReleased)
        {
            KeyIsRequiredForAnyCombinations(charReleased);
        }

        public void KeyIsRequiredForAnyCombinations(char charReleased)
        {
            List<char> charsToRemove = new List<char>();

            foreach(KeyPressRelease kpr in keysPressedAndReleased)
            {
                int numberOfRemovalsRequired = TheresKeysDataThatHasThisChar(kpr.Character, keysPressedAndReleased);

                for(int i = 0; i < numberOfRemovalsRequired;i++)
                {
                    charsToRemove.Add(kpr.Character);
                }

            }

            foreach (char characterToRemove in charsToRemove)
            {
                charsReleased.Remove(characterToRemove);
                keysPressedAndReleased.Remove(keysPressedAndReleased.First(x => x.Character == characterToRemove));
            }
        }

        private int TheresKeysDataThatHasThisChar(char character, List<KeyPressRelease> keysPressedAndReleased)
        {
            int keysUsingCharacter = UnprocessedData.Count(x => (x.FirstChar == character || x.SecondChar == character) && x.WaitingForEvents.Count != 0);
            int numberOfThisCharacterInKPR = keysPressedAndReleased.Count(x => x.Character == character);
            List<KeysData> allKeyDatasForRemoval = UnprocessedData.FindAll(x => (x.FirstChar == character || x.SecondChar == character) && x.WaitingForEvents.Count == 0);
            int keysThatCouldBeRemoved = allKeyDatasForRemoval.Count();

            if (keysUsingCharacter == numberOfThisCharacterInKPR && keysUsingCharacter == keysThatCouldBeRemoved )
            {
                return 0;
            }
            else
            {
                
                int returnValue = allKeyDatasForRemoval.Count;

                foreach(KeysData tempKeys in allKeyDatasForRemoval)
                {
                    UnprocessedData.Remove(tempKeys);
                }

                //returnValue = (returnValue>0)? returnValue-1: returnValue;
                return returnValue;
            }
        }

        private void RecordOnReleaseData(char charReleased, long timeInTicks, KeysData keysData)
        {

            keysData.WaitingForEvents.Remove(KeysData.WaitingForEvent.EFirstKeyRelease);

            keysData.HoldTime = TimeSpan.FromTicks(timeInTicks - keysCurrentlyHeld[charReleased]);

            if (NextKeyAlreadyPressed(keysData))
            {
                keysData.FlightTime = TimeSpan.FromTicks(keysPressedAndReleased.First(x => x.Character == keysData.SecondChar).TimePressedInTicks - timeInTicks);
            }

            KeysData previousKeyData = KeysDataWhereSecondCharIs(charReleased);
            if (previousKeyData != null)
            {
                previousKeyData.WaitingForEvents.Remove(KeysData.WaitingForEvent.ESecondKeyRelease);
            }

            if (previousReleaseKeysData != null)
            {
                if (KeysDataWhereSecondCharIs(charReleased) == null)
                {
                    KeyPressRelease secondKeyReleasedFirst = keysPressedAndReleased[keysPressedAndReleased.FindIndex(x => x.Character == keysData.FirstChar) + 1];

                    RemoveUnnecesarryKeys(charReleased);


                    keysData.Digraph2 = TimeSpan.FromTicks(secondKeyReleasedFirst.TimeReleasedInTicks - timeInTicks);
                    keysData.Digraph3 = GetDigraph3(keysData);

                }
                else // This happens when the second key of the key combination is released first
                {
                    previousKeyData = KeysDataWhereSecondCharIs(charReleased);

                    previousKeyData.Digraph2 = GetDigraph2(timeInTicks, previousKeyData);
                    previousKeyData.Digraph3 = GetDigraph3(previousKeyData);

                    previousKeyData.WaitingForEvents.Remove(KeysData.WaitingForEvent.ESecondKeyRelease);
                    RemoveUnnecesarryKeys(charReleased);

                    if (previousReleaseKeysData.FirstChar == keysData.SecondChar)
                    {
                        KeyPressRelease secondKeyReleasedFirst = keysPressedAndReleased.Last(x => x.Character == keysData.SecondChar);
                        keysData.Digraph2 = TimeSpan.FromTicks(secondKeyReleasedFirst.TimeReleasedInTicks - timeInTicks);
                        keysData.Digraph3 = GetDigraph3(keysData);
                    }
                }
            }
        }

        private TimeSpan GetDigraph2(long timeInTicks, KeysData keysData)
        {
            return TimeSpan.FromTicks(timeInTicks - keysPressedAndReleased.First(x => x.Character == keysData.FirstChar).TimeReleasedInTicks);
        }

        private TimeSpan GetDigraph3(KeysData keysData)
        {
            return keysData.HoldTime + keysData.Digraph2;
        }

        private bool NextKeyAlreadyPressed(KeysData keysData)
        {
            return lastKeyPress != keysData.FirstChar;
        }

        private KeysData KeysDataWhereFirstCharIs(char firstChar)
        {
            return DataRecorded.Last(x => x.FirstChar == firstChar);
        }

        private KeysData KeysDataWhereSecondCharIs(char secondChar)
        {
            return DataRecorded.LastOrDefault(x => x.SecondChar == secondChar);
        }

        private long ReleaseOfLastKey(char charReleased)
        {
            return keysPressedAndReleased.Last(x => x.Character == charReleased).TimePressedInTicks;
        }

        public void End()
        {
            lastKeyPress = '\0';
            timeOfPreviousRelease = 0;
            timeOfPreviousPress = 0;
            previousReleaseKeysData = null;
            CurrentState = SessionState.Inactive;
            timer.Enabled = false;
            DataRecorded = new List<KeysData>();
            timeSinceLastAction = 0;
            keysPressedAndReleased = new List<KeyPressRelease>();
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
