using StructuralEqualityAssessor.Internal;
using StructuralEqualityAssessor.Test.Examples;
using Xunit;

namespace StructuralEqualityAssessor.Test.TestersShould
{
    public class CheckTypeInequality
    {
        [Fact]
        public void AndReturnTrueForBasicObjects()
        {
            object target = new OtherClass();
            var result = Testers.CheckTypeInequality(typeof(OtherClass), target);

            Assert.True(result);
        }

        [Fact]
        public void AndReturnTrueForStringTypes()
        {
            var result = Testers.CheckTypeInequality(typeof(string), "Hello");

            Assert.True(result);
        }

        [Fact]
        public void AndReturnTrueForIntTypes()
        {
            var result = Testers.CheckTypeInequality(typeof(int), 5);

            Assert.True(result);
        }

        [Fact]
        public void AndReturnFalseForBadlyBehavedClass()
        {
            var result = Testers.CheckTypeInequality(typeof(BadEqualityClass), new BadEqualityClass());

            Assert.False(result);
        }
    }
}