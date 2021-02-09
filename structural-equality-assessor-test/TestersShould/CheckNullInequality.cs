using StructuralEqualityAssessor.Internal;
using StructuralEqualityAssessor.Test.Examples;
using StructuralEqualityAssessor.Test.Examples.LookUpFailingClasses;
using StructuralEqualityAssessor.Test.Examples.LookUpGoodClasses;
using Xunit;

namespace StructuralEqualityAssessor.Test.TestersShould
{
    public class CheckNullInequality
    {
        [Fact]
        public void ReturnsTrueForDefaultClass()
        {
            object target = new AnotherClass();
            var result = Testers.CheckNullInequality(target);

            Assert.True(result);
        }

        [Fact]
        public void ReturnsTrueForClassWithStructuralEquality()
        {
            object target = new OtherClassWithEquality();
            var result = Testers.CheckNullInequality(target);

            Assert.True(result);
        }

        [Fact]
        public void ReturnsTrueForClassWithValueType()
        {
            object target = new OtherValueType();
            var result = Testers.CheckNullInequality(target);

            Assert.True(result);
        }

        [Fact]
        public void ReturnsFalseForNull()
        {
            var result = Testers.CheckNullInequality(null);

            Assert.False(result);
        }
    }
}