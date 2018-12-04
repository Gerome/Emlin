using Emlin;
using NUnit.Framework;

namespace EmlinTests
{
    [TestFixture]
    class HelperFunctionsTests
    {
        [Test]
        public static void Get_combination_id_should_return_the_id_of_the_combination()
        {
            Assert.That(HelperFunctions.GetCombinationId(' ',' '), Is.EqualTo(4128));
        }

        [Test]
        public static void Passing_delete_twice_should_return_the_correct_id()
        {
            Assert.That(HelperFunctions.GetCombinationId((char)127, (char)127), Is.EqualTo(16383));
        }

     
    }
}
