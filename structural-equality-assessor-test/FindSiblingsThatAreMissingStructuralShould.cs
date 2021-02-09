using System;
using System.Collections.Generic;
using System.Linq;
using ApprovalTests;
using StructuralEqualityAssessor.Test.Examples;
using StructuralEqualityAssessor.Test.Examples.LookUpFailingClasses;
using StructuralEqualityAssessor.Test.Examples.LookUpGoodClasses;
using StructuralEqualityAssessor.Test.Examples.LookUpIgnoredClasses;
using StructuralEqualityAssessor.Test.Examples.LookUpMixedClasses;
using Xunit;

namespace StructuralEqualityAssessor.Test
{
    public class FindSiblingsThatAreMissingStructuralShould
    {
        [Fact]
        public void NotFindAnythingIfAllAreCorrect()
        {
            var results = FindSiblingsThatAreMissingStructural.Equality<HasSimpleArrays>();
            Assert.Empty(results);
        }

        [Fact]
        public void FindAllWrongWhenAllAreWrong()
        {
            var results = FindSiblingsThatAreMissingStructural.Equality<AnotherClass>();

            var friendlyResults = results.Select(t => t.ToString()).OrderBy(t => t);
            Approvals.VerifyAll(friendlyResults, "Bad Equality");
        }

        [Fact]
        public void IgnoreAllIgnoredTypes()
        {
            var ignoreThese = new[] {typeof(DynamicProperty)};
            
            var result = FindSiblingsThatAreMissingStructural.Equality<ReferenceGoodClass>(ignoreThese);
            Assert.Empty(result);
        }

        [Fact]
        public void OnlyReportFailingTypesIfMixed()
        {
            var ctors = new Dictionary<Type, Func<object>>
            {
                {typeof(NonDefaultConstructor), () => new NonDefaultConstructor(44)} 
            };
            var results = FindSiblingsThatAreMissingStructural.Equality<BadEqualityClass>(null, ctors);
            var friendlyResults = results.Select(t => t.ToString()).OrderBy(t => t);
            Approvals.VerifyAll(friendlyResults, "Bad Equality");
        }
    }
}