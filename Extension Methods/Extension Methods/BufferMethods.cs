using System;

namespace Extension_Methods
{
    public static unsafe class BufferUtil
    {
        public static void MemSet(this byte[] array, byte value)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            byte[] rawValue = { value, value, value, value, value, value, value, value };
            long fillValue = BitConverter.ToInt64(rawValue, 0);

            fixed (byte* ptr = array)
            {
                long* dest = (long*)ptr;
                int length = array.Length;
                while (length >= 8)
                {
                    *dest = fillValue;
                    dest++;
                    length -= 8;
                }
                byte* bDest = (byte*)dest;
                for (byte i = 0; i < length; i++)
                {
                    *bDest = value;
                    bDest++;
                }
            }
        }


        public static void MemSet(this byte[] array, byte value, int startIndex, int length)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (length < 0 || length > array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }
            if (startIndex < 0 || startIndex + length > array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            byte[] rawValue = { value, value, value, value, value, value, value, value };
            long fillValue = BitConverter.ToInt64(rawValue, 0);

            fixed (byte* ptr = &array[startIndex])
            {
                long* dest = (long*)ptr;
                while (length >= 8)
                {
                    *dest = fillValue;
                    dest++;
                    length -= 8;
                }
                byte* bDest = (byte*)dest;
                for (byte i = 0; i < length; i++)
                {
                    *bDest = value;
                    bDest++;
                }
            }
        }


        public static void MemCpy(byte* src, byte* dest, int len)
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            if (dest == null) throw new ArgumentNullException(nameof(dest));
            if (len >= 0x10)
            {
                do
                {
                    *((int*)dest) = *((int*)src);
                    *((int*)(dest + 4)) = *((int*)(src + 4));
                    *((int*)(dest + 8)) = *((int*)(src + 8));
                    *((int*)(dest + 12)) = *((int*)(src + 12));
                    dest += 0x10;
                    src += 0x10;
                }
                while ((len -= 0x10) >= 0x10);
            }

            if (len <= 0) return;
            if ((len & 8) != 0)
            {
                *((int*)dest) = *((int*)src);
                *((int*)(dest + 4)) = *((int*)(src + 4));
                dest += 8;
                src += 8;
            }
            if ((len & 4) != 0)
            {
                *((int*)dest) = *((int*)src);
                dest += 4;
                src += 4;
            }
            if ((len & 2) != 0)
            {
                *((short*)dest) = *((short*)src);
                dest += 2;
                src += 2;
            }

            if ((len & 1) == 0) return;
            dest++;
            src++;
            dest[0] = src[0];
        }


        public static int SizeOf(object obj)
        {
            return SizeOf(obj.GetType());
        }
    }
}
