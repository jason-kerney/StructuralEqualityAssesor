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
    public class SetFieldValues
    {
        [Fact]
        public void OnMultipleObjects()
        {
            var a = new OtherClass();
            var b = new OtherClass();
            var c = new OtherClass();

            var targets = new[] {a, b,};
            var (_, fieldValues) = Creator.GetPropertyAndFieldValues(typeof(OtherClass), null);
            Action<FieldInfo, object> noOp = (f, v) => { };
            Creator.SetFieldValues(targets, fieldValues, noOp);

            Assert.Equal(a.ToString(), b.ToString());
            Assert.NotEqual(a.ToString(), c.ToString());
        }

        [Fact]
        public void OnMultipleObjectsAndCallsDelegateOnEachField()
        {
            var a = new OtherClass();
            var fields = new List<string>();

            var targets = new[] {a,};
            var (_, fieldValues) = Creator.GetPropertyAndFieldValues(typeof(OtherClass), null);
            Action<FieldInfo, ValueAccessors> noOp = (field, value) => { fields.Add($"{field} => {value}"); };
            Creator.SetFieldValues(targets, fieldValues, noOp);

            Approvals.VerifyAll(fields, "result");
        }

        [Fact]
        public void OnComplexObjects()
        {
            var a = new SomeComplexClass();

            var targets = new[] {a,};
            var ctors = new Dictionary<Type, Func<object>>
            {
                {typeof(NonDefaultConstructor), () => new NonDefaultConstructor(8)}
            };

            var (_, fieldValues) = Creator.GetPropertyAndFieldValues(typeof(SomeComplexClass), ctors);
            Creator.SetFieldValues(targets, fieldValues, (f, v) => { });

            Approvals.Verify(a);
        }

        [Fact]
        public void OnSingleTarget()
        {
            var target = new OtherClass();

            var (_, fieldValues) = Creator.GetPropertyAndFieldValues(typeof(OtherClass), null);
            Creator.SetFieldValues(target, fieldValues);

            Approvals.Verify(target.ToString());
        }
    }
}
