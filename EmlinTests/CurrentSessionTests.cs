using Emlin;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmlinTests
{
    class CurrentSessionTests
    {
        CurrentSession testSession = new CurrentSession();

        [TearDown]
        public void Dispose()
        {
            testSession.EndSession();
        }

        [Test]
        public void PRESSING_TWO_KEYS_ADDS_ONE_COMBINATION_TO_THE_TIMESPAN_LIST()
        {
            testSession.KeyWasPressed('A', 100);
            testSession.KeyWasPressed('B', 200);

            int combId = HelperFunctions.GetCombinationId('A', 'B');

            Assert.That(testSession.KeysPressed[combId].TimeSpanList.Count, Is.EqualTo(1));
        }

        [Test]
        public void CURRENT_SESSION_IS_INACTIVE_BY_DEFAULT()
        {
            Assert.That(testSession.CurrentState, Is.EqualTo(CurrentSession.SessionState.Inactive));
        }

        [Test]
        public void PRESSING_A_KEY_SETS_THE_STATE_OF_THE_SESSION_TO_ACTIVE()
        {
            testSession.KeyWasPressed('A', 100);
            Assert.That(testSession.CurrentState, Is.EqualTo(CurrentSession.SessionState.Active));
        }

        [Test]
        public void PRESSING_2_KEY_SETS_THE_STATE_OF_THE_SESSION_TO_ACTIVE()
        {
            testSession.KeyWasPressed('A', 100);
            testSession.KeyWasPressed('B', 200);
            Assert.That(testSession.CurrentState, Is.EqualTo(CurrentSession.SessionState.Active));
        }

        [Test]
        public void PRESSING_TWO_KEYS_ADDS_A_COMBINATION_OBJECT_TO_THE_CURRENT_SESSION_COMBINATION_LIST()
        {
            testSession.KeyWasPressed('A', 100);
            testSession.KeyWasPressed('B', 200);

            int combId = HelperFunctions.GetCombinationId('A', 'B');

            Assert.That(testSession.KeysPressed[combId].TimeSpanList[0].Ticks, Is.EqualTo(100));
        }

        [Test]
        public void PRESSING_A_KEY_AND_WAITING_2_SECONDS_SHOWS_THE_SESSION_IS_INACTIVE()
        {
            int waitInSeconds = 2;
            testSession.KeyWasPressed('A', 0);
            SimulateThreadSleep(1000 * waitInSeconds);

            Assert.That(testSession.CurrentState, Is.EqualTo(CurrentSession.SessionState.Inactive));
        }

        [Test]
        public void PRESSING_TWO_KEYS_WITH_A_2_SECOND_GAP_FINISHES_THE_SESSION()
        {
            int waitInSeconds = 2;
            testSession.KeyWasPressed('A', 0);
            SimulateThreadSleep(1000 * waitInSeconds);
            testSession.KeyWasPressed('B', TimeSpan.TicksPerSecond * waitInSeconds);

            int combId = HelperFunctions.GetCombinationId('A', 'B');

            Assert.That(testSession.KeysPressed[combId], Is.Null);
        }

        private void SimulateThreadSleep(int delayInMillis)
        {
            if(delayInMillis >= ConstantValues.LENGTH_OF_SESSION_IN_MILLIS)
            {
                testSession.EndSession();
            }
        }
    }
}
