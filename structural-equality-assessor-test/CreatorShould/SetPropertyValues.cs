using System;
using System.Collections.Generic;
using System.Reflection;
using ApprovalTests;
using ApprovalTests.Reporters;
using StructuralEqualityAssessor.Internal;
using StructuralEqualityAssessor.Test.Examples;
using Xunit;

namespace StructuralEqualityAssessor.Test.CreatorShould
{
    [UseReporter(typeof(DiffReporter))]
    public class SetPropertyValues
    {
        [Fact]
        public void OnMultipleObjects()
        {
            var a = new OtherClass();
            var b = new OtherClass();
            var c = new OtherClass();

            var targets = new[] {a, b};
            var (propertyValues, _) = Creator.GetPropertyAndFieldValues(typeof(OtherClass), null);
            Action<PropertyInfo, object> noOp = (p, v) => { };

            Creator.SetPropertyValues(targets, propertyValues, noOp);

            Assert.Equal(a.ToString(), b.ToString());
            Assert.NotEqual(a.ToString(), c.ToString());
        }

        [Fact]
        public void OnMultipleObjectsAndExecutesDelegateOnEachProperty()
        {
            var a = new OtherClass();
            var properties = new List<string>();

            var targets = new[] {a};
            var (propertyValues, _) = Creator.GetPropertyAndFieldValues(typeof(OtherClass), null);
            Action<PropertyInfo, ValueAccessors> collect = (property, value) => properties.Add($"{property} => {value}");

            Creator.SetPropertyValues(targets, propertyValues, collect);

            Approvals.VerifyAll(properties, "result");
        }

        [Fact]
        public void OnComplexObjects()
        {
            var a = new SomeComplexClass();

            var targets = new[] {a};
            var ctors = new Dictionary<Type, Func<object>>
            {
                {typeof(NonDefaultConstructor), () => new NonDefaultConstructor(6)}
            };
            var (propertyValues, _) = Creator.GetPropertyAndFieldValues(typeof(SomeComplexClass), ctors);

            Creator.SetPropertyValues(targets, propertyValues, (p, v) => { });

            Approvals.Verify(a);
        }

        [Fact]
        public void OnSingleObject()
        {
            var target = new OtherClass();
            var (propertyValues, _) = Creator.GetPropertyAndFieldValues(typeof(OtherClass), null);
            Creator.SetPropertyValues(target, propertyValues);

            Approvals.Verify(target.ToString());
        }
    }
}
