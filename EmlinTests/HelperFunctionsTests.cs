using Emlin;
using NUnit.Framework;

namespace EmlinTests
{
    [TestFixture]
    class HelperFunctionsTests
    {
        [Test]
        public static void GET_COMBINATION_ID_SHOULD_RETURN_THE_ID_OF_THE_COMBINATION()
        {
            Assert.That(HelperFunctions.GetCombinationId(' ',' '), Is.EqualTo(3104));
        }

        [Test]
        public static void PASSING_DELETE_TWICE_SHOULD_RETURN_THE_CORRECT_ID()
        {
            Assert.That(HelperFunctions.GetCombinationId((char)127, (char)127), Is.EqualTo(12319));
        }
    }
}
