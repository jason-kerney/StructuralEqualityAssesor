namespace StructuralEqualityAssessor.EqualityHelpers
{
    /// <summary>
    /// A utility to evaluate if arrays are structurally equal.
    /// </summary>
    public static class ArraysAreStructurally
    {
        /// <summary>
        /// Returns whether or not 2 arrays hold the same values in the same order.
        /// </summary>
        /// <typeparam name="T">The type of the arrays.</typeparam>
        /// <param name="left">An array to compare</param>
        /// <param name="right">An array to compare</param>
        /// <returns>True, if each array contains the same number of items each with the same value in the same order.</returns>
        public static bool Equal<T>(T[] left, T[] right)
        {
            if (left == null && right != null) return false;
            if (left != null && right == null) return false;
            if (left == null) return true;
            if (ReferenceEquals(left, right)) return true;

            var sameLengths = left.Length == right.Length;

            var contentsEqual = true;
            for (var i = 0; sameLengths && i < left.Length; i++)
            {
                contentsEqual = contentsEqual && left[i].Equals(right[i]);
            }

            return
                sameLengths
                && contentsEqual;
        }
    }
}