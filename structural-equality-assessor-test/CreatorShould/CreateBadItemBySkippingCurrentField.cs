using System;
using System.Collections.Generic;
using System.Linq;
using ApprovalTests;
using ApprovalTests.Reporters;
using StructuralEqualityAssessor.Internal;
using StructuralEqualityAssessor.Test.Examples;
using Xunit;

namespace StructuralEqualityAssessor.Test.CreatorShould
{
    [UseReporter(typeof(DiffReporter))]
    public class CreateBadItemBySkippingCurrentField
    {
        [Fact]
        public void OfType()
        {
            Func<object> ctor = () => new OtherClass();
            var (propertyValues, fieldValues) = Creator.GetPropertyAndFieldValues(typeof(OtherClass), null);
            var result = Creator.CreateBadItemBasedOnFields(ctor, propertyValues, fieldValues, fieldValues.First().Key);

            Approvals.VerifyAll(result, "result");
        }

        [Fact]
        public void OfTypeDiffField()
        {
            Func<object> ctor = () => new OtherClass();
            var (propertyValues, fieldValues) = Creator.GetPropertyAndFieldValues(typeof(OtherClass), null);
            var result = Creator.CreateBadItemBasedOnFields(ctor, propertyValues, fieldValues, fieldValues.Last().Key);

            Approvals.VerifyAll(result, "result");
        }

        [Fact]
        public void OfTypeWithArrays()
        {
            Func<object> ctor = () => new HasArrays();
            var ctors = new Dictionary<Type, Func<object>>
            {
                {typeof(NonDefaultConstructor), () => new NonDefaultConstructor(77)},
            };

            var (propertyValues, fieldValues) = Creator.GetPropertyAndFieldValues(typeof(HasArrays), ctors);
            var result = Creator.CreateBadItemBasedOnFields(ctor, propertyValues, fieldValues, fieldValues.Last().Key);

            Approvals.VerifyAll(result, "result");
        }
    }
}