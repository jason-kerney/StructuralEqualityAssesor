using System.Collections.Generic;
using StructuralEqualityAssessor.Internal;
using StructuralEqualityAssessor.Test.Examples;
using StructuralEqualityAssessor.Test.Examples.LookUpFailingClasses;
using StructuralEqualityAssessor.Test.Examples.LookUpGoodClasses;
using Xunit;

namespace StructuralEqualityAssessor.Test.TestersShould
{
    public class CheckInequality
    {
        [Fact]
        public void ReturnsTrueForBasicObjectsEvenIfStructurallyTheSame()
        {
            IEnumerable<object> others = new []
            {
                new AnotherClass{ someChar = 'T', someByte = 32, SomeNumber = 42, SomeString = "String"},
                new AnotherClass{ someChar = 'T', someByte = 32, SomeNumber = 42, SomeString = "String"},
                new AnotherClass{ someChar = 'T', someByte = 32, SomeNumber = 42, SomeString = "String"},
            };
            object populatedItem = new AnotherClass{ someChar = 'T', someByte = 32, SomeNumber = 42, SomeString = "String"};
            var result = Testers.CheckInequality(others, populatedItem);

            Assert.True(result);
        }

        [Fact]
        public void ReturnsFalseForBasicObjectsIfOneOfThemIsThePopulatedItem()
        {
            object populatedItem = new AnotherClass{ someChar = 'T', someByte = 32, SomeNumber = 42, SomeString = "String"};
            IEnumerable<object> others = new []
            {
                new AnotherClass{ someChar = 'T', someByte = 32, SomeNumber = 42, SomeString = "String"},
                populatedItem,
                new AnotherClass{ someChar = 'T', someByte = 32, SomeNumber = 42, SomeString = "String"},
            };
            var result = Testers.CheckInequality(others, populatedItem);

            Assert.False(result);
        }

        [Fact]
        public void ReturnsTrueForObjectsThatDifferStructurally()
        {
            IEnumerable<object> others = new[]
            {
                new OtherClassWithEquality {SomeNumber = 39},
                new OtherClassWithEquality {SomeNumber = 40},
                new OtherClassWithEquality {SomeNumber = 41},
            };
            object populatedItem = new OtherClassWithEquality {SomeNumber = 42};
            var result = Testers.CheckInequality(others, populatedItem);

            Assert.True(result);
        }

        [Fact]
        public void ReturnsFalseIfOneObjectIsStructurallyTheSame()
        {
            IEnumerable<object> others = new[]
            {
                new OtherClassWithEquality {SomeNumber = 42},
                new OtherClassWithEquality {SomeNumber = 40},
                new OtherClassWithEquality {SomeNumber = 41},
            };
            object populatedItem = new OtherClassWithEquality {SomeNumber = 42};
            var result = Testers.CheckInequality(others, populatedItem);

            Assert.False(result);
        }

        [Fact]
        public void ReturnsFalseIfOneObjectIsThePopulatedObject()
        {
            object populatedItem = new OtherClassWithEquality {SomeNumber = 42};
            IEnumerable<object> others = new[]
            {
                new OtherClassWithEquality {SomeNumber = 42},
                new OtherClassWithEquality {SomeNumber = 40},
                populatedItem,
            };
            var result = Testers.CheckInequality(others, populatedItem);

            Assert.False(result);
        }

        [Fact]
        public void ReturnsFalseIfNonOfTheObjectsMatchTheTypeOfThePopulatedItem()
        {
            object populatedItem = new OtherClassWithEquality {SomeNumber = 42};
            IEnumerable<object> others = new object[]
            {
                "Hello",
                5,
                'A',
            };
            var result = Testers.CheckInequality(others, populatedItem);

            Assert.True(result);
        }
    }
}