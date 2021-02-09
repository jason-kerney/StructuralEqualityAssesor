using System;

namespace StructuralEqualityAssessor.Test.Examples.LookUpIgnoredClasses
{
    public class ReferenceGoodClass : IEquatable<ReferenceGoodClass>
    {
        public bool Equals(ReferenceGoodClass other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Number == other.Number;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ReferenceGoodClass) obj);
        }

        public override int GetHashCode()
        {
            return Number;
        }

        public static bool operator ==(ReferenceGoodClass left, ReferenceGoodClass right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ReferenceGoodClass left, ReferenceGoodClass right)
        {
            return !Equals(left, right);
        }

        public int Number { get; set; }
    }
}