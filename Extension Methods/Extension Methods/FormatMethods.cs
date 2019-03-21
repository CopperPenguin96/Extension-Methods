using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension_Methods
{
    public static unsafe class FormatMethods
    {
        // Quicker StringBuilder.Append(int) by Sam Allen of http://www.dotnetperls.com
        public static StringBuilder Digits(this StringBuilder builder, int number)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (number >= 100000000 || number < 0)
            {
                // Use system ToString.
                builder.Append(number);
                return builder;
            }
            int copy;
            int digit;
            if (number >= 10000000)
            {
                // 8.
                copy = number % 100000000;
                digit = copy / 10000000;
                builder.Append((char)(digit + 48));
            }
            if (number >= 1000000)
            {
                // 7.
                copy = number % 10000000;
                digit = copy / 1000000;
                builder.Append((char)(digit + 48));
            }
            if (number >= 100000)
            {
                // 6.
                copy = number % 1000000;
                digit = copy / 100000;
                builder.Append((char)(digit + 48));
            }
            if (number >= 10000)
            {
                // 5.
                copy = number % 100000;
                digit = copy / 10000;
                builder.Append((char)(digit + 48));
            }
            if (number >= 1000)
            {
                // 4.
                copy = number % 10000;
                digit = copy / 1000;
                builder.Append((char)(digit + 48));
            }
            if (number >= 100)
            {
                // 3.
                copy = number % 1000;
                digit = copy / 100;
                builder.Append((char)(digit + 48));
            }
            if (number >= 10)
            {
                // 2.
                copy = number % 100;
                digit = copy / 10;
                builder.Append((char)(digit + 48));
            }

            if (number < 0) return builder;
            // 1.
            copy = number % 10;
            builder.Append((char)(copy + 48));
            return builder;
        }

        // Quicker Int32.Parse(string) by Karl Seguin
        public static int Parse(string stringToConvert)
        {
            if (stringToConvert == null) throw new ArgumentNullException(nameof(stringToConvert));
            int value = 0;
            int length = stringToConvert.Length;
            fixed (char* characters = stringToConvert)
            {
                for (int i = 0; i < length; ++i)
                {
                    value = 10 * value + (characters[i] - 48);
                }
            }
            return value;
        }

        // UppercaseFirst by Sam Allen of http://www.dotnetperls.com
        public static string UppercaseFirst(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
    }
}
