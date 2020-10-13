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
    public class GetPropertyAndFieldValues
    {
        [Fact]
        public void OfValueType()
        {
            var (propertyValuesFromType, fieldValuesFromType) = Creator.GetPropertyAndFieldValues(typeof(OtherValueType), null);

            var valueStrings = propertyValuesFromType.Select(kvp => $"{kvp.Key} => {kvp.Value}").ToList();

            valueStrings.AddRange(fieldValuesFromType.Select(kvp => $"{kvp.Key} => {kvp.Value}"));

            Approvals.VerifyAll(valueStrings, "results");
        }

        [Fact]
        public void OfClassType()
        {
            var (propertyValuesFromType, fieldValuesFromType) = Creator.GetPropertyAndFieldValues(typeof(OtherClass), null);

            var valueStrings = propertyValuesFromType.Select(kvp => $"{kvp.Key} => {kvp.Value}").ToList();

            valueStrings.AddRange(fieldValuesFromType.Select(kvp => $"{kvp.Key} => {kvp.Value}"));

            Approvals.VerifyAll(valueStrings, "results");
        }

        [Fact]
        public void OfClassTypeWithArrays()
        {
            var ctors = new Dictionary<Type, Func<object>>
            {
                {typeof(NonDefaultConstructor), () => new NonDefaultConstructor(33)}
            };
            var (propertyValuesFromType, fieldValuesFromType) = Creator.GetPropertyAndFieldValues(typeof(HasArrays), ctors);

            var valueStrings = propertyValuesFromType.Select(kvp => $"{kvp.Key} => {kvp.Value}").ToList();

            valueStrings.AddRange(fieldValuesFromType.Select(kvp => $"{kvp.Key} => {kvp.Value}"));

            Approvals.VerifyAll(valueStrings, "results");
        }
    }
}
