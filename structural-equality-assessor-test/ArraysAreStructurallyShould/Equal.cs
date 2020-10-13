﻿using StructuralEqualityAssessor.EqualityHelpers;
using StructuralEqualityAssessor.Test.Examples;
using Xunit;

namespace StructuralEqualityAssessor.Test.ArraysAreStructurallyShould
{
    public class Equal
    {
        [Fact]
        public void ReturnTrueForIntArraysContainingSameValues()
        {
            var left = new[] {3, 4, 5};
            var right = new[] {3, 4, 5};

            var result = ArraysAreStructurally.Equal(left, right);

            Assert.True(result);
        }

        [Fact]
        public void ReturnFalseForIntArraysNotContainingSameValues()
        {
            var left = new[] {3, 4, 6};
            var right = new[] {3, 4, 5};

            var result = ArraysAreStructurally.Equal(left, right);

            Assert.False(result);
        }

        [Fact]
        public void ReturnFalseForIntArraysContainingDifferentNumberOfValues()
        {
            var left = new[] {3, 4, 5};
            var right = new[] {3, 4, 5, 6};

            var result = ArraysAreStructurally.Equal(left, right);

            Assert.False(result);
        }

        [Fact]
        public void ReturnFalseForIntArraysContainingSameNumberValuesDifferentOrder()
        {
            var left = new[] {3, 4, 5};
            var right = new[] {3, 5, 4};

            var result = ArraysAreStructurally.Equal(left, right);

            Assert.False(result);
        }

        [Fact]
        public void ReturnFalseForIntArraysWhenLeftIsNull()
        {
            var right = new[] {3, 4, 5, 6};

            var result = ArraysAreStructurally.Equal(null, right);

            Assert.False(result);
        }

        [Fact]
        public void ReturnFalseForIntArraysWhenRightIsNull()
        {
            var left = new[] {3, 4, 5, 6};

            var result = ArraysAreStructurally.Equal(left, null);

            Assert.False(result);
        }

        [Fact]
        public void ReturnTrueIfBothRightAndLeftAreSameArray()
        {
            var a = new[] {3, 4, 5, 6};

            var result = ArraysAreStructurally.Equal(a, a);

            Assert.True(result);
        }

        [Fact]
        public void ReturnFalseForArraysContainingBasicObjects()
        {
            var left = new[] {new OtherClass(), new OtherClass(), new OtherClass(), };
            var right = new[] {new OtherClass(), new OtherClass(), new OtherClass(), };

            var result = ArraysAreStructurally.Equal(left, right);

            Assert.False(result);
        }

        [Fact]
        public void ReturnTrueForArraysContainingSameBasicObjects()
        {
            var a = new OtherClass();
            var b = new OtherClass();
            var c = new OtherClass();
            var left = new[] {a, b, c, };
            var right = new[] {a, b, c, };

            var result = ArraysAreStructurally.Equal(left, right);

            Assert.True(result);
        }

        [Fact]
        public void ReturnTrueForArraysContainingSimilarStructuralEqualObjects()
        {
            var left  = new[] {new OtherClassWithEquality{SomeNumber = 1}, new OtherClassWithEquality{SomeNumber = 2}, new OtherClassWithEquality{SomeNumber = 3}, };
            var right = new[] {new OtherClassWithEquality{SomeNumber = 1}, new OtherClassWithEquality{SomeNumber = 2}, new OtherClassWithEquality{SomeNumber = 3}, };

            var result = ArraysAreStructurally.Equal(left, right);

            Assert.True(result);
        }
    }
}