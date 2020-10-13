using System;

namespace StructuralEqualityAssessor.Test.Examples
{
    public class OtherClassWithEquality : IEquatable<OtherClassWithEquality>
    {
        public bool Equals(OtherClassWithEquality other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return SomeNumber == other.SomeNumber;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((OtherClassWithEquality) obj);
        }

        public override int GetHashCode()
        {
            return SomeNumber;
        }

        public static bool operator ==(OtherClassWithEquality left, OtherClassWithEquality right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(OtherClassWithEquality left, OtherClassWithEquality right)
        {
            return !Equals(left, right);
        }

        public int SomeNumber { get; set; }

        public override string ToString()
        {
            return $"{{ {nameof(SomeNumber)}: {SomeNumber} }}";
        }
    }
}