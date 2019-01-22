

namespace Emlin
{
    public class KeyPressRelease
    {
        public char Character { get; }

        public long TimePressedInTicks { get; }

        public long TimeReleasedInTicks { get; set; }

        public KeyPressRelease(char character, long timePressedInTicks)
        {
            Character = character;
            this.TimePressedInTicks = timePressedInTicks;
        }

    }
}
