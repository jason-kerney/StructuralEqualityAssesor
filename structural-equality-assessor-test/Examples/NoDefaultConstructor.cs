namespace StructuralEqualityAssessor.Test.Examples
{
    internal class NoDefaultConstructor
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public int SomeNumber { get; set; }

        public NoDefaultConstructor(int someNumber)
        {
            SomeNumber = someNumber;
        }
    }
}