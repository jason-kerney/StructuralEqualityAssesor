using System;
using System.Collections.Generic;
using System.Reflection;
using ApprovalTests;
using ApprovalTests.Reporters;
using StructuralEqualityAssessor.Internal;
using StructuralEqualityAssessor.Test.Examples;
using StructuralEqualityAssessor.Test.Examples.LookUpFailingClasses;
using StructuralEqualityAssessor.Test.Examples.LookUpMixedClasses;
using Xunit;

namespace StructuralEqualityAssessor.Test.CreatorShould
{
    [UseReporter(typeof(DiffReporter))]
    public class SetPropertyValues
    {
        [Fact]
        public void OnMultipleObjects()
        {
            var a = new AnotherClass();
            var b = new AnotherClass();
            var c = new AnotherClass();

            var targets = new[] {a, b};
            var (propertyValues, _) = Creator.GetPropertyAndFieldValues(typeof(AnotherClass), null);
            Action<PropertyInfo, object> noOp = (p, v) => { };

            Creator.SetPropertyValues(targets, propertyValues, noOp);

            Assert.Equal(a.ToString(), b.ToString());
            Assert.NotEqual(a.ToString(), c.ToString());
        }

        [Fact]
        public void OnMultipleObjectsAndExecutesDelegateOnEachProperty()
        {
            var a = new AnotherClass();
            var properties = new List<string>();

            var targets = new[] {a};
            var (propertyValues, _) = Creator.GetPropertyAndFieldValues(typeof(AnotherClass), null);
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
            var target = new AnotherClass();
            var (propertyValues, _) = Creator.GetPropertyAndFieldValues(typeof(AnotherClass), null);
            Creator.SetPropertyValues(target, propertyValues);

            Approvals.Verify(target.ToString());
        }
    }
}
