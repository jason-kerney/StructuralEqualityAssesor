using StructuralEqualityAssessor.Internal;
using StructuralEqualityAssessor.Test.Examples;
using StructuralEqualityAssessor.Test.Examples.LookUpFailingClasses;
using StructuralEqualityAssessor.Test.Examples.LookUpGoodClasses;
using Xunit;

namespace StructuralEqualityAssessor.Test.TestersShould
{
    public class CheckEquality
    {
        [Fact]
        public void AndReturnsFalseForDefaultObjectEvenIfStructurallyEqual()
        {
            object left = new AnotherClass{ SomeNumber = 5, SomeString = "High-Low", someByte = 24, someChar = 'Z'};
            object right = new AnotherClass{ SomeNumber = 5, SomeString = "High-Low", someByte = 24, someChar = 'Z'};
            var result = Testers.CheckEquality(left, right);

            Assert.False(result);
        }

        [Fact]
        public void AndReturnsTrueForForTheSameBasicObject()
        {
            object thing = new AnotherClass { SomeNumber = 5};
            var result = Testers.CheckEquality(thing, thing);

            Assert.True(result);
        }

        [Fact]
        public void AndReturnsTrueForObjectIfStructurallyEqual()
        {
            object left = new OtherClassWithEquality { SomeNumber = 5};
            object right = new OtherClassWithEquality{ SomeNumber = 5};
            var result = Testers.CheckEquality(left, right);

            Assert.True(result);
        }

        [Fact]
        public void AndReturnsFalseForObjectIfNotStructurallyEqual()
        {
            object left = new OtherClassWithEquality { SomeNumber = 5};
            object right = new OtherClassWithEquality{ SomeNumber = 6};
            var result = Testers.CheckEquality(left, right);

            Assert.False(result);
        }

        [Fact]
        public void AndReturnsFalseIfLeftObjectIsNull()
        {
            object right = new OtherClassWithEquality{ SomeNumber = 6};
            var result = Testers.CheckEquality(null, right);

            Assert.False(result);
        }

        [Fact]
        public void AndReturnsFalseIfRightObjectIsNull()
        {
            object left = new OtherClassWithEquality{ SomeNumber = 6};
            var result = Testers.CheckEquality(left, null);

            Assert.False(result);
        }

        [Fact]
        public void AndReturnsFalseIfBothAreNull()
        {
            var result = Testers.CheckEquality(null, null);

            Assert.False(result);
        }

        [Fact]
        public void AndReturnsFalseIfBothAreDifferentTypes()
        {
            object left = new OtherClassWithEquality();
            var result = Testers.CheckEquality(left, "Hello");

            Assert.False(result);
        }
    }
}