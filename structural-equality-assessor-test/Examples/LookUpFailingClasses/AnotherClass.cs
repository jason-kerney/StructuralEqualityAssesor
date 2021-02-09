namespace StructuralEqualityAssessor.Test.Examples.LookUpFailingClasses
{
    public class AnotherClass
    {
        public byte someByte;
        public char someChar;
        public int SomeNumber { get; set; }
        public string SomeString { get; set; }

        public override string ToString()
        {
            return $"{{ {nameof(someByte)}: {someByte}, {nameof(someChar)}: '{someChar}', {nameof(SomeNumber)}: {SomeNumber}, {nameof(SomeString)}: {StringToString(SomeString)} }}";
        }

        private static string StringToString(string value)
        {
            return 
                value == null 
                    ? "null"
                    : $"\"{value}\"";
        }
    }
}