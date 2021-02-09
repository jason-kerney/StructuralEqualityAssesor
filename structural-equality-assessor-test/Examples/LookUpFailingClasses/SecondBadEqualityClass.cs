using System;

namespace StructuralEqualityAssessor.Test.Examples.LookUpFailingClasses
{
    public class SecondBadEqualityClass : IEquatable<SecondBadEqualityClass>
    {
        public bool Equals(SecondBadEqualityClass other)
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

        public static bool operator ==(SecondBadEqualityClass left, SecondBadEqualityClass right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SecondBadEqualityClass left, SecondBadEqualityClass right)
        {
            return !Equals(left, right);
        }

        public int Num { get; set; }
    }
}