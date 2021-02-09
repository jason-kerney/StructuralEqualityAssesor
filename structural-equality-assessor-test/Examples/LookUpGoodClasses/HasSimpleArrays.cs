using System;
using System.Linq;
using StructuralEqualityAssessor.EqualityHelpers;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace StructuralEqualityAssessor.Test.Examples.LookUpGoodClasses
{
    public class HasSimpleArrays : IEquatable<HasSimpleArrays>
    {
        public bool Equals(HasSimpleArrays other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return
                ArraysAreStructurally.Equal(otherNumbers, other.otherNumbers)
                && ArraysAreStructurally.Equal(Numbers, other.Numbers);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HasSimpleArrays) obj);
        }

        public override int GetHashCode()
        {
            var otherNumbersHashCodes = otherNumbers?.Aggregate(HashCode.Combine) ?? 0;
            var numbersHashCode = Numbers?.Aggregate(HashCode.Combine) ?? 0;
            return HashCode.Combine(otherNumbersHashCodes, numbersHashCode);
        }

        public static bool operator ==(HasSimpleArrays left, HasSimpleArrays right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(HasSimpleArrays left, HasSimpleArrays right)
        {
            return !Equals(left, right);
        }

        public int[] Numbers { get; set; }
        public int[] otherNumbers;

        public override string ToString()
        {
            static string ArrayToString<T>(T[] array)
            {
                return 
                    array == null 
                        ? "null" 
                        : $"[{string.Join(", ", array)}]";
            }

            return $"{{ {nameof(Numbers)}: {ArrayToString(Numbers)}, {nameof(otherNumbers)}: {ArrayToString(otherNumbers)} }}";
        }
    }
}