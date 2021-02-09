using System;
using System.Collections.Generic;
using System.Linq;
using ApprovalTests;
using ApprovalTests.Reporters;
using StructuralEqualityAssessor.Internal;
using StructuralEqualityAssessor.Test.Examples;
using StructuralEqualityAssessor.Test.Examples.LookUpFailingClasses;
using StructuralEqualityAssessor.Test.Examples.LookUpGoodClasses;
using Xunit;

namespace StructuralEqualityAssessor.Test.CreatorShould
{
    [UseReporter(typeof(DiffReporter))]
    public class CreateBadItemBySkippingCurrentProperty
    {
        [Fact]
        public void OfType()
        {
            Func<object> ctor = () => new AnotherClass();
            var (propertyValues, fieldValues) = Creator.GetPropertyAndFieldValues(typeof(AnotherClass), null);
            var result = Creator.CreateBadItemBasedOnProperty(ctor, propertyValues, fieldValues, propertyValues.First().Key);

            Approvals.VerifyAll(result, "result");
        }

        [Fact]
        public void OfTypeWithDifferentProperty()
        {
            Func<object> ctor = () => new AnotherClass();
            var (propertyValues, fieldValues) = Creator.GetPropertyAndFieldValues(typeof(AnotherClass), null);
            var result = Creator.CreateBadItemBasedOnProperty(ctor, propertyValues, fieldValues, propertyValues.Last().Key);

            Approvals.VerifyAll(result, "result");
        }

        [Fact]
        public void OfTypeWithArrays()
        {
            Func<object> ctor = () => new HasArrays();
            var ctors = new Dictionary<Type, Func<object>>
            {
                {typeof(NonDefaultConstructor), () => new NonDefaultConstructor(66)}
            };
            var (propertyValues, fieldValues) = Creator.GetPropertyAndFieldValues(typeof(HasArrays), ctors);
            var result = Creator.CreateBadItemBasedOnProperty(ctor, propertyValues, fieldValues, propertyValues.Last().Key);

            Approvals.VerifyAll(result, "result");
        }
    }
}
