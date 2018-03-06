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
            Assert.That(HelperFunctions.GetCombinationId(' ',' '), Is.EqualTo(0));
        }
    }
}
