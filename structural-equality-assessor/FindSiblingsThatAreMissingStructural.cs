using System;
using System.Collections.Generic;
using System.Linq;

namespace StructuralEqualityAssessor
{
    /// <summary>
    /// A utility to examine all types in a given namespace and find all instances that do not implement structural equality.
    /// </summary>
    public static class FindSiblingsThatAreMissingStructural
    {
        /// <summary>
        /// Returns all types from the namespace of the given type which do not support structural equality.
        /// </summary>
        /// <typeparam name="TGuide">The type used to get the assembly and namespace to check.</typeparam>
        /// <param name="ignoreThese">A collection of types to not check.</param>
        /// <param name="constructors">Instructions on how to construct types without default constructors. It is preferred that it constructs the type with some value that differs from some default value.</param>
        /// <returns>A list of types who do not support structural equality.</returns>
        public static IEnumerable<Type> Equality<TGuide>(IEnumerable<Type> ignoreThese = null, IDictionary<Type, Func<object>> constructors = null)
        {
            var tType = typeof(TGuide);
            var nspace = tType.Namespace;

            var filters =
                ignoreThese
                ?? new Type[0];

            var ctors =
                constructors
                ?? new Dictionary<Type, Func<object>>();

            var toBeChecked =
                from 
                    t in tType.Assembly.GetTypes()
                where
                    t.Namespace == nspace
                    && !filters.Contains(t)
                    && !t.IsAbstract
                    && !t.IsInterface
                    && t.IsPublic
                select t;

            return
                from t in toBeChecked
                where !HasStructural.Equality(t, ctors)
                select t;
        }
    }
}