using System;
using System.Collections.Generic;
using ApprovalTests;
using ApprovalTests.Reporters;
using StructuralEqualityAssessor.Internal;
using StructuralEqualityAssessor.Test.Examples;
using StructuralEqualityAssessor.Test.Examples.LookUpFailingClasses;
using StructuralEqualityAssessor.Test.Examples.LookUpGoodClasses;
using StructuralEqualityAssessor.Test.Examples.LookUpMixedClasses;
using Xunit;

namespace StructuralEqualityAssessor.Test.CreatorShould
{
        [UseReporter(typeof(DiffReporter))]
        public class SetUpObject
        {
            [Fact]
            public void OfValueType()
            {
                var target = (object)new OtherValueType();

                Assert.Equal(0, ((OtherValueType) target).SomeNumber);

                Creator.SetUpObject(ref target, null);

                Assert.Equal(1, ((OtherValueType) target).SomeNumber);
            }

            [Fact]
            public void OfNullExitsGracefully()
            {
                var target = (object)null;

                Creator.SetUpObject(ref target, null);

                Assert.Null(target);
            }

            [Fact]
            public void OfClass()
            {
                var target = (object)new AnotherClass();

                Assert.Equal(0, ((AnotherClass) target).SomeNumber);
                Assert.Null(((AnotherClass) target).SomeString);
                Assert.Equal((byte)0, ((AnotherClass) target).someByte);
                Assert.Equal('\0', ((AnotherClass) target).someChar);

                Creator.SetUpObject(ref target, null);

                Assert.Equal(1, ((AnotherClass) target).SomeNumber);
                Assert.Equal("A string", ((AnotherClass) target).SomeString);
                Assert.Equal((byte)1, ((AnotherClass) target).someByte);
                Assert.Equal('A', ((AnotherClass) target).someChar);
            }

            [Fact]
            public void OfComplexClass()
            {
                var target = (object)new SomeComplexClass();

                Assert.Null(((SomeComplexClass) target).Thing);
                Assert.Null(((SomeComplexClass) target).Thing2);

                var ctors = new Dictionary<Type, Func<object>>
                {
                    {typeof(NonDefaultConstructor), () => new NonDefaultConstructor(4)}
                };
                Creator.SetUpObject(ref target, ctors);

                Assert.NotNull(((SomeComplexClass) target).Thing);
                Assert.NotNull(((SomeComplexClass) target).Thing2);
                Assert.Equal(1, ((SomeComplexClass) target).Thing.SomeNumber);
                Assert.Equal(1, ((SomeComplexClass) target).Thing2.ANumber);
            }

            [Fact]
            public void OfTypeWithArrays()
            {
                var target = (object)new HasArrays();

                Assert.Null(((HasArrays) target).Numbers);
                var ctors = new Dictionary<Type, Func<object>>
                {
                    {typeof(NonDefaultConstructor), () => new NonDefaultConstructor(99)}
                };

                Creator.SetUpObject(ref target, ctors);

                Approvals.Verify(target);
            }
        }
    }
