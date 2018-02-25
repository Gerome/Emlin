using NUnit.Framework;
using Emlin;
using System.Collections;

namespace EmlinTests
{
    [TestFixture]
    public class EmlinKeyboardTest
    {
        [Test]
        public void KEYBOARD_RECORDER_SHOULD_CREATE_AN_ARRAY()
        {
            KeyboardRecorder kbRec = new KeyboardRecorder();
            char[] listOfInputs = kbRec.ListOfInputs;
            
            Assert.NotNull(listOfInputs, null);
        }
    }
}
