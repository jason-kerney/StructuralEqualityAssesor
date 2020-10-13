using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StructuralEqualityAssessor.Internal
{
    /// <summary>
    /// A utility to construct a type with non-default values.
    /// </summary>
    public static class Creator
    {
        /// <summary>
        /// Creates instance of a type and populates it with non-default values.
        /// </summary>
        /// <param name="t">The type to construct</param>
        /// <param name="constructors">Valid way to construct either the given type, or included types that do not contain default constructors.</param>
        /// <returns>An instance of the given type.</returns>
        public static object CreateInstanceOfType(this Type t, IDictionary<Type, Func<object>> constructors = null)
        {
            if ((constructors?.ContainsKey(t)).GetValueOrDefault())
            {
                // ReSharper disable once PossibleNullReferenceException
                return constructors[t]();
            }

            if (t.IsArray)
            {
                var elementType = t.GetElementType();
                if (elementType == null)
                {
                    return null;
                }

                return Array.CreateInstance(elementType, 1);
            }

            // Use non default values for built in types
            if (t == typeof(string)) return "A string";

            if (t == typeof(int)      || t == typeof(int?))      return 1;
            if (t == typeof(short)    || t == typeof(short?))    return (short)1;
            if (t == typeof(byte)     || t == typeof(byte?))     return (byte)1;
            if (t == typeof(sbyte)    || t == typeof(sbyte?))    return (sbyte)1;
            if (t == typeof(double)   || t == typeof(double?))   return (double)1;
            if (t == typeof(long)     || t == typeof(long?))     return (long)1;
            if (t == typeof(float)    || t == typeof(float?))    return (float)1;
            if (t == typeof(decimal)  || t == typeof(decimal?))  return (decimal)1;
            if (t == typeof(ushort)   || t == typeof(ushort?))   return (ushort)1;
            if (t == typeof(ulong)    || t == typeof(ulong?))    return (ulong)1;
            if (t == typeof(uint)     || t == typeof(uint?))     return (uint)1;
            if (t == typeof(bool)     || t == typeof(bool?))     return true;
            if (t == typeof(char)     || t == typeof(char?))     return 'A';
            if (t == typeof(DateTime) || t == typeof(DateTime?)) return DateTime.Parse("3/14/1592 6:53:58.97");

            object result;

            if (t.IsValueType)
            {
                result = Activator.CreateInstance(t);
            }
            else
            {
                var a = t.GetConstructor(new Type[0]);
                if (a == null || a.IsGenericMethod)
                    throw new ArgumentException($"{t} cannot be constructed");

                result = a.Invoke(null);
            }

            return result;
        }

        /// <summary>
        /// Populates an object with non-default values
        /// </summary>
        /// <param name="target">The object to populate. Does not populate default .Net System value types or strings.</param>
        /// <param name="constructors">A way to construct valid instances of contained types.</param>
        public static void SetUpObject(ref object target, IDictionary<Type, Func<object>> constructors = null)
        {
            if (target == null) return;
            
            var type = target.GetType();

            if(type.IsValueType && type.Namespace == typeof(int).Namespace || type == typeof(string)) return;

            if (type.IsArray)
            {
                var instanceOfType = type.GetElementType().CreateInstanceOfType(constructors);
                SetUpObject(ref instanceOfType, constructors);
                ((Array)target).SetValue(instanceOfType, 0);
                return;
            }

            var (properties, fields) = GetPropertyAndFieldValues(type, constructors);
            SetPropertyValues(target, properties);
            SetFieldValues(target, fields);
        }

        /// <summary>
        /// Gets non-default values for every set-able property and field values of a given type
        /// </summary>
        /// <param name="tType">The target type to get property and field values for.</param>
        /// <param name="constructors">A way to construct types for property and field types that do not have a default constructor.</param>
        /// <returns>A collection of Property and Field values with corresponding info allowing caller to easily set them.</returns>
        public static (Dictionary<PropertyInfo, ValueAccessors> propertyValuesFromType, Dictionary<FieldInfo, ValueAccessors> fieldValuesFromType) GetPropertyAndFieldValues(Type tType, IDictionary<Type, Func<object>> constructors = null)
        {
            var properties =
                tType
                    .GetProperties(BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance)
                    .Where(p => !p.PropertyType.IsGenericTypeDefinition)
                    .ToArray();

            var fields = tType.GetFields(BindingFlags.Public | BindingFlags.SetField | BindingFlags.Instance);

            var propertyValuesFromType =
                properties
                    .Select(property =>
                    {
                        var propertyPropertyType = property.PropertyType;
                        object CreatePopulateInstance()
                        {
                            var instance = propertyPropertyType.CreateInstanceOfType(constructors);
                            SetUpObject(ref instance, constructors);
                            return instance;
                        }

                        var emptyValues = new List<Func<object>>
                        {
                            () =>
                            {
                                if (propertyPropertyType.IsValueType)
                                    return Activator.CreateInstance(propertyPropertyType);

                                if (propertyPropertyType == typeof(string))
                                    return null;

                                if (propertyPropertyType.IsArray)
                                {
                                    var elementType = propertyPropertyType.GetElementType();
                                    if (elementType == null)
                                        return null;
                                    
                                    return Array.CreateInstance(elementType, 0);
                                }

                                return propertyPropertyType.CreateInstanceOfType(constructors);
                            }
                        };

                        if (propertyPropertyType.IsArray)
                        {
                            emptyValues.Add(() => null);
                        }

                        return new {property, accessor = new ValueAccessors { GetPopulatedValue = CreatePopulateInstance, GetEmptyValues = emptyValues}};
                    })
                    .ToDictionary(
                        thing => thing.property,
                        thing => thing.accessor
                    );

            var fieldValuesFromType =
                fields
                    .Select(field =>
                    {
                        var fieldFieldType = field.FieldType;
                        object GetPopulated()
                        {
                            var instance = fieldFieldType.CreateInstanceOfType(constructors);
                            SetUpObject(ref instance, constructors);
                            return instance;
                        }

                        var emptyValues = new List<Func<object>>
                        {
                            () =>
                            {
                                if (fieldFieldType.IsValueType)
                                    return Activator.CreateInstance(fieldFieldType);

                                if (fieldFieldType == typeof(string))
                                    return null;

                                if (fieldFieldType.IsArray)
                                {
                                    var elementType = fieldFieldType.GetElementType();
                                    if (elementType == null)
                                        return null;
                                    
                                    return Array.CreateInstance(elementType, 0);
                                }

                                return fieldFieldType.CreateInstanceOfType(constructors);
                            }
                        };

                        if (fieldFieldType.IsArray)
                        {
                            emptyValues.Add(() => null);
                        }

                        return new {field, accessor = new ValueAccessors { GetPopulatedValue = GetPopulated, GetEmptyValues = emptyValues }};
                    })
                    .ToDictionary(
                        thing => thing.field,
                        thing => thing.accessor
                    );

            var values = (propertyValuesFromType, fieldValuesFromType);
            return values;
        }

        /// <summary>
        /// Creates a number of items who's specified field is empty or default equal to the number of ways it can be empty/default. Eg. An array can be both an empty array or null.
        /// </summary>
        /// <param name="constructor">A valid way to construct the type who's values will be populated.</param>
        /// <param name="propertyValues">A list of valid non-default property values to apply to the objects.</param>
        /// <param name="fieldValues">A list of valid non-default field values to apply to the objects.</param>
        /// <param name="field">The field who's value will be empty/default.</param>
        /// <returns>A number of items with the given field empty/default for each way it can be empty/default.</returns>
        public static IEnumerable<object> CreateBadItemBasedOnFields(Func<object> constructor, Dictionary<PropertyInfo, ValueAccessors> propertyValues, Dictionary<FieldInfo, ValueAccessors> fieldValues, FieldInfo field)
        {
            var items = new List<object>();
            var emptyValueAccessors = fieldValues[field].GetEmptyValues;

            foreach (var emptyAccessor in emptyValueAccessors)
            {
                var item = constructor();
                SetPropertyValues(item, propertyValues);

                foreach (var (key, currentValue) in fieldValues)
                {
                    key.SetValue(
                        item,
                        key != field
                            ? currentValue.GetPopulatedValue()
                            : emptyAccessor()
                    );
                }

                items.Add(item);
            }

            return items;
        }

        /// <summary>
        /// Creates a number of items who's specified property is empty or default equal to the number of ways it can be empty/default. Eg. An array can be both an empty array or null.
        /// </summary>
        /// <param name="constructor">A valid way to construct the type who's values will be populated.</param>
        /// <param name="propertyValues">A list of valid non-default property values to apply to the objects.</param>
        /// <param name="fieldValues">A list of valid non-default field values to apply to the objects.</param>
        /// <param name="property">The property who's value will be empty/default.</param>
        /// <returns>A number of items with the given property empty/default for each way it can be empty/default.</returns>
        public static IEnumerable<object> CreateBadItemBasedOnProperty(Func<object> constructor, Dictionary<PropertyInfo, ValueAccessors> propertyValues, Dictionary<FieldInfo, ValueAccessors> fieldValues, PropertyInfo property)
        {
            var items = new List<object>();
            var getEmpties = propertyValues[property].GetEmptyValues;

            foreach (var getEmpty in getEmpties)
            {
                var item = constructor();
                foreach (var (key, currentValue) in propertyValues)
                {
                    SetFieldValues(item, fieldValues);

                    if (key != property)
                    {
                        key.SetValue(item, currentValue.GetPopulatedValue());
                    }
                    else
                    {
                        key.SetValue(item, getEmpty());
                    }
                }

                items.Add(item);
            }

            return items;
        }

        /// <summary>
        /// Sets each target's properties based on the the property values provided.
        /// </summary>
        /// <param name="targets">A collection of targets who are the same type, and same type as specified by the property values.</param>
        /// <param name="propertyValues">A collection of property info's and the value to set the property to.</param>
        /// <param name="doMore">Allows additional processing of the property and value.</param>
        public static void SetPropertyValues(IEnumerable<object> targets, IEnumerable<KeyValuePair<PropertyInfo, ValueAccessors>> propertyValues, Action<PropertyInfo, ValueAccessors> doMore)
        {
            foreach (var (property, value) in propertyValues)
            {
                foreach (var target in targets)
                {
                    property.SetValue(target, value.GetPopulatedValue());
                }

                doMore(property, value);
            }
        }

        /// <summary>
        /// Sets the properties on an object to those given in property values.
        /// </summary>
        /// <param name="target">The target who's properties to set.</param>
        /// <param name="propertyValues">A collection of property info and value to set the property to.</param>
        public static void SetPropertyValues(object target, Dictionary<PropertyInfo, ValueAccessors> propertyValues)
        {
            SetPropertyValues(new[] {target}, propertyValues, (p, v) => { });
        }

        /// <summary>
        /// Sets each target's fields based on the the field values provided.
        /// </summary>
        /// <param name="targets">A collection of targets who are the same type, and same type as specified by the field values.</param>
        /// <param name="fieldValues">A collection of field info's and the value to set the field to.</param>
        /// <param name="doMore">Allows additional processing of the field and value.</param>
        public static void SetFieldValues(IEnumerable<object> targets, IEnumerable<KeyValuePair<FieldInfo, ValueAccessors>> fieldValues, Action<FieldInfo, ValueAccessors> doMore)
        {
            foreach (var (field, value) in fieldValues)
            {
                foreach (var target in targets)
                {
                    field.SetValue(target, value.GetPopulatedValue());
                }

                doMore(field, value);
            }
        }

        /// <summary>
        /// Sets the fields on an object to those given in field values.
        /// </summary>
        /// <param name="target">The target who's fields to set.</param>
        /// <param name="fieldValues">A collection of field info and value to set the field to.</param>
        public static void SetFieldValues(object target, Dictionary<FieldInfo, ValueAccessors> fieldValues)
        {
            SetFieldValues(new[] {target}, fieldValues, (property, value) => {});
        }
    }
}