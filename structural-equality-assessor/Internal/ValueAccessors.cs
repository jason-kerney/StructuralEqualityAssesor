using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StructuralEqualityAssessor.Internal
{
    /// <summary>
    /// A class intended to hold possible values for fields and properties.
    /// </summary>
    public class ValueAccessors
    {
        /// <summary>
        /// A function that returns a known valid non-default value for the property or field.
        /// </summary>
        public Func<object> GetPopulatedValue { get; set; }

        /// <summary>
        /// A function that returns a collection of known empty and or default values for the property or field.
        /// </summary>
        public IEnumerable<Func<object>> GetEmptyValues { get; set; }

        /// <summary>
        /// A string representation of all values.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            static string ValueToString(object value)
            {
                if (value == null) return "null";

                var type = value.GetType();

                if (typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string))
                {
                    var array = (IEnumerable)value;
                    var items = 
                        from object item in array 
                        select ValueToString(item);

                    return "[" + string.Join(", ", items) + "]";
                }

                var quote = string.Empty;
                if (type == typeof(string))
                {
                    quote = "\"";
                }

                if (type == typeof(char))
                {
                    quote = "'";
                }

                return quote + value + quote;
            }

            string GetEmptyValuesToString()
            {
                var thing = GetEmptyValues?.Select(f => ValueToString(f()));

                return 
                    thing == null 
                        ? "null" 
                        : $"[{string.Join(", ", thing)}]";
            }


            return $"{{ Populated: {ValueToString(GetPopulatedValue())}, Empty: {GetEmptyValuesToString()} }}";
        }
    }
}