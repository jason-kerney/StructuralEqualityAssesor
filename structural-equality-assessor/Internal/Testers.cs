using System;
using System.Collections.Generic;
using System.Linq;

namespace StructuralEqualityAssessor.Internal
{
    /// <summary>
    /// A utility to test In/equality between objects
    /// </summary>
    public static class Testers
    {
        /// <summary>
        /// Verifies equality has been implemented such that is correctly identifies two objects as not equal.
        /// </summary>
        /// <param name="left">A object to compare</param>
        /// <param name="right">A object to compare</param>
        /// <returns>True if the objects are correctly not equal.</returns>
        public static bool CheckInequality(object left, object right)
        {
            try
            {
                var notEqualOp = (bool)((dynamic)left != (dynamic)right);
                var notEqualOp2 = (bool)((dynamic)right != (dynamic)left);
                var equalOp = (((dynamic) left == (dynamic) right) == false);
                var equalOp2 = (((dynamic) right == (dynamic) left) == false);
                var equalMethod = (!left.Equals(right));
                var equalMethod2 = (!right.Equals(left));

                return
                    notEqualOp
                    && notEqualOp2
                    && equalOp
                    && equalOp2
                    && equalMethod
                    && equalMethod2;
            }
            catch
            {
                return true;
            }
        }

        /// <summary>
        /// Verifies equality has been implemented such that it correctly identifies an object as not null.
        /// </summary>
        /// <param name="target">The object to compare to null.</param>
        /// <returns>True if the object is correctly not equal to null.</returns>
        public static bool CheckNullInequality(object target)
        {
            var notEqualOp = (target != null);
            var notEqualOp2 = (null != target);

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            var equalOp = ((target == null) == false);
            var equalOp2 = ((null == target) == false);

            // ReSharper disable once PossibleNullReferenceException
            var equalMethod = (!target?.Equals(null)).GetValueOrDefault();

            return
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                notEqualOp
                && notEqualOp2
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                && equalOp
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                && equalOp2
                && equalMethod;
        }

        /// <summary>
        /// Verifies equality has been implemented such that it correctly identifies 2 objects as being equal.
        /// </summary>
        /// <param name="left">An object to compare</param>
        /// <param name="right">An object to compare</param>
        /// <returns>True if equality correctly identifies 2 objects as equal.</returns>
        public static bool CheckEquality(object left, object right)
        {
            try
            {
                var equalOp = (bool) ((dynamic) left == (dynamic) right);
                var equalOp2 = (bool) ((dynamic) right == (dynamic) left);
                var notEqualOp = (bool) (((dynamic) left != (dynamic) right) == false);
                var notEqualOp2 = (bool) (((dynamic) right != (dynamic) left) == false);
                var equalMethod = (left?.Equals(right)).GetValueOrDefault();
                var equalMethod2 = (right?.Equals(left)).GetValueOrDefault();
                var hashCode = left?.GetHashCode() == right?.GetHashCode();

                return
                    equalOp
                    && equalOp2
                    && notEqualOp
                    && notEqualOp2
                    && equalMethod
                    && equalMethod2
                    && hashCode;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Verifies that equality handles wrong type correctly, by verifying that the type is not simultaneously a string and an int, while having the value "Hello" if not a string, or the value 5 if not an int.
        /// </summary>
        /// <param name="tType">The type of the target</param>
        /// <param name="target">The object to compare.</param>
        /// <returns>True if equality correctly handles different types.</returns>
        public static bool CheckTypeInequality(Type tType, object target)
        {
            var typeEqualityString =
                tType == typeof(string)
                || !target.Equals("Hello");

            var typeEqualityInt =
                tType == typeof(int)
                || tType == typeof(int?)
                || !target.Equals(5);

            var typeEquality =
                typeEqualityInt
                && typeEqualityString;
            return typeEquality;
        }

        /// <summary>
        /// Verifies equality correctly implemented across many items.
        /// </summary>
        /// <param name="itemsWithMissingValues">Items that all differ from the populated item in some way.</param>
        /// <param name="populatedItem">A base item to compare all items against.</param>
        /// <returns>True if equality is implemented such that all items with missing values differ from the populated item.</returns>
        public static bool CheckInequality(IEnumerable<object> itemsWithMissingValues, object populatedItem)
        {
            return itemsWithMissingValues
                .Aggregate(true, (result, item) =>
                    result
                    && Testers.CheckInequality(populatedItem, item)
                );
        }
    }
}