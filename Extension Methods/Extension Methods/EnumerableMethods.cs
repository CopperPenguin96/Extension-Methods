using System;
using System.Collections.Generic;
using System.Text;

namespace Extension_Methods
{
    public static class EnumerableMethods
    {
        /// <summary> Joins all items in a collection into one comma-separated string.
        /// If the items are not strings, .ToString() is called on them. </summary>
        public static string JoinToString<T>(this IEnumerable<T> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            StringBuilder sb = new StringBuilder();
            bool first = true;
            foreach (T item in items)
            {
                if (!first) sb.Append(',').Append(' ');
                sb.Append(item);
                first = false;
            }
            return sb.ToString();
        }


        /// <summary> Joins all items in a collection into one string separated with the given separator.
        /// If the items are not strings, .ToString() is called on them. </summary>
        public static string JoinToString<T>(this IEnumerable<T> items, string separator)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (separator == null) throw new ArgumentNullException(nameof(separator));
            StringBuilder sb = new StringBuilder();
            bool first = true;
            foreach (T item in items)
            {
                if (!first) sb.Append(separator);
                sb.Append(item);
                first = false;
            }
            return sb.ToString();
        }


        /// <summary> Joins all items in a collection into one string separated with the given separator.
        /// A specified string conversion function is called on each item before contactenation. </summary>
        public static string JoinToString<T>(this IEnumerable<T> items, Func<T, string> stringConversionFunction)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (stringConversionFunction == null) throw new ArgumentNullException(nameof(stringConversionFunction));
            StringBuilder sb = new StringBuilder();
            bool first = true;
            foreach (T item in items)
            {
                if (!first) sb.Append(',').Append(' ');
                sb.Append(stringConversionFunction(item));
                first = false;
            }
            return sb.ToString();
        }


        /// <summary> Joins all items in a collection into one string separated with the given separator.
        /// A specified string conversion function is called on each item before contactenation. </summary>
        public static string JoinToString<T>(this IEnumerable<T> items, string separator, Func<T, string> stringConversionFunction)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (separator == null) throw new ArgumentNullException(nameof(separator));
            if (stringConversionFunction == null) throw new ArgumentNullException(nameof(stringConversionFunction));
            StringBuilder sb = new StringBuilder();
            bool first = true;
            foreach (T item in items)
            {
                if (!first) sb.Append(separator);
                sb.Append(stringConversionFunction(item));
                first = false;
            }
            return sb.ToString();
        }
    }
}
