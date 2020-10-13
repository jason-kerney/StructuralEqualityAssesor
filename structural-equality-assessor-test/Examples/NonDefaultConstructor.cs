using System;

namespace StructuralEqualityAssessor.Test.Examples
{
    public class NonDefaultConstructor : IEquatable<NonDefaultConstructor>
    {
        public bool Equals(NonDefaultConstructor other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ANumber == other.ANumber;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NonDefaultConstructor) obj);
        }

        public override int GetHashCode()
        {
            return ANumber;
        }

        public static bool operator ==(NonDefaultConstructor left, NonDefaultConstructor right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NonDefaultConstructor left, NonDefaultConstructor right)
        {
            return !Equals(left, right);
        }

        public NonDefaultConstructor(int aNumber)
        {
            ANumber = aNumber;
        }

        public int ANumber { get; set; }

        public override string ToString()
        {
            return $"{{ {nameof(ANumber)}: {ANumber} }}";
        }
    }
}