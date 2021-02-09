using System;
using StructuralEqualityAssessor.Test.Examples.LookUpGoodClasses;

namespace StructuralEqualityAssessor.Test.Examples.LookUpMixedClasses
{
    public class SomeComplexClass : IEquatable<SomeComplexClass>
    {
        public bool Equals(SomeComplexClass other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return aString == other.aString && aChar == other.aChar && Equals(Thing, other.Thing) && Equals(Thing2, other.Thing2);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SomeComplexClass) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(aString, aChar, Thing, Thing2);
        }

        public static bool operator ==(SomeComplexClass left, SomeComplexClass right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SomeComplexClass left, SomeComplexClass right)
        {
            return !Equals(left, right);
        }

        public OtherClassWithEquality Thing { get; set; }

        public NonDefaultConstructor Thing2 { get; set; }

        public string aString;

        public char aChar;

        public override string ToString()
        {
            string AStringToString()
            {
                if (aString == null)
                    return "null";

                return "\"" + aString + "\"";
            }

            return $"{{ {nameof(Thing)}: {Thing?.ToString() ?? "null"}, {nameof(Thing2)}: {Thing2?.ToString() ?? "null"}, {nameof(aString)}: {AStringToString()}, {nameof(aChar)}: '{aChar}' }}";
        }
    }
}