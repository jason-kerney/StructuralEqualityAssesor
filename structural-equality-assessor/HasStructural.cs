using System;
using System.Collections.Generic;
using System.Reflection;
using StructuralEqualityAssessor.Internal;


namespace StructuralEqualityAssessor
{
    /// <summary>
    /// A static class to help to evaluate if structural equality has been implemented correctly in a given type.
    /// </summary>
    public static class HasStructural
    {
        /// <summary>
        /// Determines if a given type has implemented structural equality correctly.
        /// </summary>
        /// <param name="t">The type to test</param>
        /// <returns>True if and only if the type meets requirements for structural equality.</returns>
        public static bool Equality(Type t)
        {
            return Equality(t, new Dictionary<Type, Func<object>> {{t, () => t.CreateInstanceOfType()}});
        }

        /// <summary>
        /// Determines if a given type has implemented structural equality correctly.
        /// </summary>
        /// <param name="t">The type to test</param>
        /// <param name="constructors">a way to construct valid types for items without a default constructor, or where a default constructor is ineffective for testing structural equality.</param>
        /// <returns>True if and only if the type meets requirements for structural equality.</returns>
        public static bool Equality(Type t, IDictionary<Type, Func<object>> constructors)
        {
            if (!constructors.ContainsKey(t))
            {
                constructors[t] = () => t.CreateInstanceOfType();
            }

            var emptyWorks = TestEmptyTypes(constructors[t]);
            var works = emptyWorks && TestPopulatedTypes(t, constructors);

            return works;
        }

        private static bool TestPopulatedTypes(Type t, IDictionary<Type, Func<object>> constructors)
        {
            var ctor = constructors[t];
            var a = ctor();
            var b = ctor();
            var c = ctor();
            
            var equalMembers = new[] { a, b, c};

            var tType = a.GetType();

            var (propertyValues, fieldValues) = Creator.GetPropertyAndFieldValues(tType, constructors);

            var memberUnequalItems = SetupEqualityChecksBasedOnProperties(propertyValues, equalMembers, ctor, fieldValues);
            var badFields = SetupEqualityChecksBasedOnFields(fieldValues, equalMembers, ctor, propertyValues);

            memberUnequalItems.AddRange(badFields);

            var empty = ctor();

            var emptyInequality = Testers.CheckInequality(a, empty);

            var bEquality = Testers.CheckEquality(a, b);
            var bCommutativeEquality = Testers.CheckEquality(b, a);

            var cEquality = Testers.CheckEquality(a, c);
            var associativeEquality = Testers.CheckEquality(b, c);

            var memberInequality = Testers.CheckInequality(memberUnequalItems, a);

            var nullInequality = Testers.CheckNullInequality(a);
            var selfEquality = Testers.CheckEquality(a, a);

            var typeInequality = Testers.CheckTypeInequality(tType, a);

            return
                nullInequality
                && emptyInequality
                && bEquality
                && bCommutativeEquality
                && cEquality
                && associativeEquality
                && memberInequality
                && selfEquality
                && typeInequality;
        }

        private static bool TestEmptyTypes(Func<object> constructor)
        {
            var a = constructor();
            var b = constructor();

            var correctNullEquality = Testers.CheckNullInequality(a);
            var correctEquality = Testers.CheckEquality(a, b);
            var commutativeEquality = Testers.CheckEquality(b, a);

            return correctNullEquality
                   && correctEquality
                   && commutativeEquality;
        }

        private static IEnumerable<object> SetupEqualityChecksBasedOnFields(Dictionary<FieldInfo, ValueAccessors> fieldValues, IEnumerable<object> equalMembers, Func<object> ctor, Dictionary<PropertyInfo, ValueAccessors> propertyValues)
        {
            var bad2 = new List<object>();
            Creator.SetFieldValues(equalMembers, fieldValues, (field, value) =>
            {
                var items = Creator.CreateBadItemBasedOnFields(ctor, propertyValues, fieldValues, field);

                bad2.AddRange(items);
            });
            return bad2;
        }

        private static List<object> SetupEqualityChecksBasedOnProperties(Dictionary<PropertyInfo, ValueAccessors> propertyValues, IEnumerable<object> equalMembers, Func<object> ctor, Dictionary<FieldInfo, ValueAccessors> fieldValues)
        {
            var bad = new List<object>();
            Creator.SetPropertyValues(equalMembers, propertyValues, (property, value) =>
            {
                var badItems = Creator.CreateBadItemBasedOnProperty(ctor, propertyValues, fieldValues, property);
                bad.AddRange(badItems);
            });
            return bad;
        }
    }
}