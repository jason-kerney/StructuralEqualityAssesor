using System;
using System.Collections.Generic;
using StructuralEqualityAssessor.Internal;
using StructuralEqualityAssessor.Test.Examples;
using StructuralEqualityAssessor.Test.Examples.LookUpFailingClasses;
using StructuralEqualityAssessor.Test.Examples.LookUpMixedClasses;
using Xunit;

namespace StructuralEqualityAssessor.Test.CreatorShould
{
    public class CreateInstance
    {
        [Fact]
        public void OfTypeInt()
        {
            var result = typeof(int).CreateInstanceOfType();

            Assert.Equal(1, result);
        }

        [Fact]
        public void OfTypeNullableInt()
        {
            var result = typeof(int?).CreateInstanceOfType();

            Assert.Equal(1, result);
        }

        [Fact]
        public void OfTypeShort()
        {
            var result = typeof(short).CreateInstanceOfType();

            Assert.Equal((short) 1, result);
        }

        [Fact]
        public void OfTypeNullableShort()
        {
            var result = typeof(short?).CreateInstanceOfType();

            Assert.Equal((short) 1, result);
        }

        [Fact]
        public void OfTypeByte()
        {
            var result = typeof(byte).CreateInstanceOfType();

            Assert.Equal((byte) 1, result);
        }

        [Fact]
        public void OfTypeNullableByte()
        {
            var result = typeof(byte?).CreateInstanceOfType();

            Assert.Equal((byte) 1, result);
        }

        [Fact]
        public void OfTypeSbyte()
        {
            var result = typeof(sbyte).CreateInstanceOfType();

            Assert.Equal((sbyte) 1, result);
        }

        [Fact]
        public void OfTypeNullableSbyte()
        {
            var result = typeof(sbyte?).CreateInstanceOfType();

            Assert.Equal((sbyte) 1, result);
        }

        [Fact]
        public void OfTypeDouble()
        {
            var result = typeof(double).CreateInstanceOfType();

            Assert.Equal((double) 1, result);
        }

        [Fact]
        public void OfTypeNullableDouble()
        {
            var result = typeof(double?).CreateInstanceOfType();

            Assert.Equal((double) 1, result);
        }

        [Fact]
        public void OfTypeLong()
        {
            var result = typeof(long).CreateInstanceOfType();

            Assert.Equal((long) 1, result);
        }

        [Fact]
        public void OfTypeNullableLong()
        {
            var result = typeof(long?).CreateInstanceOfType();

            Assert.Equal((long) 1, result);
        }

        [Fact]
        public void OfTypeFloat()
        {
            var result = typeof(float).CreateInstanceOfType();

            Assert.Equal((float) 1, result);
        }

        [Fact]
        public void OfTypeNullableFloat()
        {
            var result = typeof(float?).CreateInstanceOfType();

            Assert.Equal((float) 1, result);
        }

        [Fact]
        public void OfTypeDecimal()
        {
            var result = typeof(decimal).CreateInstanceOfType();

            Assert.Equal((decimal) 1, result);
        }

        [Fact]
        public void OfTypeNullableDecimal()
        {
            var result = typeof(decimal?).CreateInstanceOfType();

            Assert.Equal((decimal) 1, result);
        }

        [Fact]
        public void OfTypeUshort()
        {
            var result = typeof(ushort).CreateInstanceOfType();

            Assert.Equal((ushort) 1, result);
        }

        [Fact]
        public void OfTypeNullableUshort()
        {
            var result = typeof(ushort?).CreateInstanceOfType();

            Assert.Equal((ushort) 1, result);
        }

        [Fact]
        public void OfTypeUlong()
        {
            var result = typeof(ulong).CreateInstanceOfType();

            Assert.Equal((ulong) 1, result);
        }

        [Fact]
        public void OfTypeNullableUlong()
        {
            var result = typeof(ulong?).CreateInstanceOfType();

            Assert.Equal((ulong) 1, result);
        }

        [Fact]
        public void OfTypeUint()
        {
            var result = typeof(uint).CreateInstanceOfType();

            Assert.Equal((uint) 1, result);
        }

        [Fact]
        public void OfTypeNullableUint()
        {
            var result = typeof(uint?).CreateInstanceOfType();

            Assert.Equal((uint) 1, result);
        }

        [Fact]
        public void OfTypeBool()
        {
            var result = typeof(bool).CreateInstanceOfType();

            Assert.Equal(true, result);
        }

        [Fact]
        public void OfTypeNullableBool()
        {
            var result = typeof(bool?).CreateInstanceOfType();

            Assert.Equal(true, result);
        }

        [Fact]
        public void OfTypeChar()
        {
            var result = typeof(char).CreateInstanceOfType();

            Assert.Equal('A', result);
        }

        [Fact]
        public void OfTypeNullableChar()
        {
            var result = typeof(char?).CreateInstanceOfType();

            Assert.Equal('A', result);
        }

        [Fact]
        public void OfTypeDateTime()
        {
            var result = typeof(DateTime).CreateInstanceOfType();

            Assert.Equal(DateTime.Parse("3/14/1592 6:53:58.97"), result);
        }

        [Fact]
        public void OfTypeNullableDateTime()
        {
            var result = typeof(DateTime?).CreateInstanceOfType();

            Assert.Equal(DateTime.Parse("3/14/1592 6:53:58.97"), result);
        }

        [Fact]
        public void OfTypeHello()
        {
            var result = typeof(string).CreateInstanceOfType();

            Assert.Equal("A string", result);
        }

        [Fact]
        public void OfTypeValueType()
        {
            var result = (OtherValueType) typeof(OtherValueType).CreateInstanceOfType();

            Assert.Equal(0, result.SomeNumber);
        }

        [Fact]
        public void OfTypeValueTypeWithGivenConstructor()
        {
            var constructors = new Dictionary<Type, Func<object>>
            {
                {typeof(OtherValueType), () => new OtherValueType {SomeNumber = 33}}
            };
            var result = (OtherValueType) typeof(OtherValueType).CreateInstanceOfType(constructors);

            Assert.Equal(33, result.SomeNumber);
        }

        [Fact]
        public void OfTypeNullableValueType()
        {
            var result = typeof(OtherValueType?).CreateInstanceOfType();

            Assert.Null(result);
        }

        [Fact]
        public void OfTypeAClass()
        {
            var result = (AnotherClass) typeof(AnotherClass).CreateInstanceOfType();

            Assert.Equal(0, result.SomeNumber);
            Assert.Null(result.SomeString);
            Assert.Equal((byte) 0, result.someByte);
            Assert.Equal('\0', result.someChar);
        }

        [Fact]
        public void OfTypeComplexClass()
        {
            var ctors = new Dictionary<Type, Func<object>>
            {
                {typeof(NonDefaultConstructor), () => new NonDefaultConstructor(4)}
            };

            var result = (SomeComplexClass) typeof(SomeComplexClass).CreateInstanceOfType(ctors);

            Assert.Null(result.Thing);
            Assert.Null(result.Thing2);
        }

        [Fact]
        public void OfTypeAClassWithoutConstructor()
        {
            var ex = Assert.Throws<ArgumentException>(() => typeof(NoDefaultConstructor).CreateInstanceOfType());
            Assert.Contains($"{nameof(NoDefaultConstructor)} cannot be constructed", ex.Message);
        }
    }
}