using System;
using System.Linq;
using StructuralEqualityAssessor.EqualityHelpers;

namespace StructuralEqualityAssessor.Test.Examples
{
    public class HasArrays : IEquatable<HasArrays>
    {
        public bool Equals(HasArrays other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return 
                ArraysAreStructurally.Equal(otherNumbers, other.otherNumbers) 
                && ArraysAreStructurally.Equal(Numbers, other.Numbers) 
                && ArraysAreStructurally.Equal(Classes, other.Classes);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HasArrays) obj);
        }

        public override int GetHashCode()
        {
            var otherNumbersHashCodes = otherNumbers?.Aggregate(HashCode.Combine) ?? 0;
            var numbersHashCode = Numbers?.Aggregate(HashCode.Combine) ?? 0;
            var classesHashCode = Classes?.Aggregate(0, HashCode.Combine) ?? 0;
            return HashCode.Combine(otherNumbersHashCodes, numbersHashCode, classesHashCode);
        }

        public static bool operator ==(HasArrays left, HasArrays right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(HasArrays left, HasArrays right)
        {
            return !Equals(left, right);
        }

        public int[] Numbers { get; set; }
        public SomeComplexClass[] Classes { get; set; }
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

            return $"{{ {nameof(Numbers)}: {ArrayToString(Numbers)}, {nameof(Classes)}: {ArrayToString(Classes)}, {nameof(otherNumbers)}: {ArrayToString(otherNumbers)} }}";
        }
    }
}