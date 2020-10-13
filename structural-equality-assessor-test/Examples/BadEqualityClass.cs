using System;

namespace StructuralEqualityAssessor.Test.Examples
{
    public class BadEqualityClass : IEquatable<BadEqualityClass>
    {
        public bool Equals(BadEqualityClass other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            return true;
        }

        public override int GetHashCode()
        {
            return Num;
        }

        public static bool operator ==(BadEqualityClass left, BadEqualityClass right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BadEqualityClass left, BadEqualityClass right)
        {
            return !Equals(left, right);
        }

        public int Num { get; set; }
    }
}