using System;
using System.Collections.Generic;
using StructuralEqualityAssessor.Test.Examples;
using Xunit;

namespace StructuralEqualityAssessor.Test.HasStructuralShould
{
    public class Equality
    {
        [Fact]
        public void BeFalseForBasicType()
        {
            var result = HasStructural.Equality(typeof(OtherClass));

            Assert.False(result);
        }

        [Fact]
        public void BeTrueForTypeThatImplementsStructuralEquality()
        {
            var result = HasStructural.Equality(typeof(OtherClassWithEquality));

            Assert.True(result);
        }

        [Fact]
        public void BeFalseForBasicTypeWithConstructorPassed()
        {
            var ctors = new Dictionary<Type, Func<object>>
            {
                {typeof(OtherClass), () => new OtherClass{SomeNumber = 11, SomeString = "12", someChar = (char)13, someByte = 14}}
            };

            var result = HasStructural.Equality(typeof(OtherClass), ctors);

            Assert.False(result);
        }

        [Fact]
        public void BeTrueForTypeThatImplementsStructuralEqualityWithConstructorPassedIn()
        {
            var ctors = new Dictionary<Type, Func<object>>
            {
                {typeof(OtherClassWithEquality), () => new OtherClassWithEquality{ SomeNumber = 789 }},
            };
            var result = HasStructural.Equality(typeof(OtherClassWithEquality), ctors);

            Assert.True(result);
        }

        [Fact]
        public void BeTrueForComplexTypeThatImplementsStructuralEqualityWithConstructorPassedIn()
        {
            var ctors = new Dictionary<Type, Func<object>>
            {
                {typeof(NonDefaultConstructor), () => new NonDefaultConstructor(5)}
            };
            var result = HasStructural.Equality(typeof(SomeComplexClass), ctors);

            Assert.True(result);
        }

        [Fact]
        public void BeTrueForTypeWithArrays()
        {
            var ctors = new Dictionary<Type, Func<object>>
            {
                {typeof(NonDefaultConstructor), () => new NonDefaultConstructor(5)}
            };
            var result = HasStructural.Equality(typeof(HasArrays), ctors);

            Assert.True(result);
        }
    }
}