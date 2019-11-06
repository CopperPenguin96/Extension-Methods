using System;

namespace Extension_Methods
{
    public static class ArrayUtil
    {
        public static void Fill<T>(this T[] array, int start, int end, T value)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (start < 0 || start >= end)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            if (end >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(end));
            }

            for (int i = start; i < end; i++)
            {
                array[i] = value;
            }
        }

        public static void Fill<T>(this T[] array, T value)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            for (int x = 0; x <= array.Length - 1; x++)
            {
                array[x] = value;
            }
        }

        public static void Copy<T>(this T[] array,
            int srcIndex, T[] dest, int destIndex, int length)
        {
            Array.Copy(array, srcIndex, dest, destIndex, length);
        }
    }
}
