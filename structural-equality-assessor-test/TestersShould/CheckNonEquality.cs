using StructuralEqualityAssessor.Internal;
using StructuralEqualityAssessor.Test.Examples;
using Xunit;

namespace StructuralEqualityAssessor.Test.TestersShould
{
    public class CheckNonEquality
    {
        [Fact]
        public void ReturnsTrueForDefaultClassEvenIfStructurallyEqual()
        {
            object left  = new OtherClass {SomeNumber = 5, SomeString = "Hi", someByte = 2, someChar = 'B'};
            object right = new OtherClass {SomeNumber = 5, SomeString = "Hi", someByte = 2, someChar = 'B'};
            var result = Testers.CheckInequality(left, right);

            Assert.True(result);
        }

        [Fact]
        public void ReturnsFalseForClassImplementingStructuralEqualityAndIsStructuralEqual()
        {
            object left  = new OtherClassWithEquality {SomeNumber = 5};
            object right = new OtherClassWithEquality {SomeNumber = 5};
            var result = Testers.CheckInequality(left, right);

            Assert.False(result);
        }

        [Fact]
        public void ReturnsTrueForClassImplementingStructuralEqualityAndIsNotStructuralEqual()
        {
            object left  = new OtherClassWithEquality {SomeNumber = 5};
            object right = new OtherClassWithEquality {SomeNumber = 6};
            var result = Testers.CheckInequality(left, right);

            Assert.True(result);
        }
    }
}