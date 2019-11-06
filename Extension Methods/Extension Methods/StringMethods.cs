using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Extension_Methods
{
    public static class StringMethods
    {
        public static bool IsValidURL(this string str)
        {
            bool result = Uri.TryCreate(str, UriKind.Absolute, out var uriResult)
                          && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }

        public static byte[] ToBytes(this string str, [Optional] Encoding enc)
        {
            if (enc == null)
            {
                enc = Encoding.UTF8;
            }

            return enc.GetBytes(str);
        }

        public static byte[] ToBytes(this string str, [Optional] Encoding enc, out int length)
        {
            byte[] b = ToBytes(str, enc);
            length = b.Length;
            return b;
        }

        public static int GetByteLength(this string str, [Optional] Encoding enc)
        {
            if (enc == null)
            {
                enc = Encoding.UTF8;
            }

            ToBytes(str, enc, out int length);
            return length;
        }

        public static string[] Alphabet =
        {
            "a", "b", "c", "d", "e", "f", "g", "h", "i",
            "j", "k", "l", "m", "n", "o", "p", "q", "r",
            "s", "t", "u", "v", "w", "x", "y", "z"
        };

        public static Image ToImage(this string str)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            byte[] array = Convert.FromBase64String(str);
            Image image = Image.FromStream(new MemoryStream(array));
            return image;
        }

        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static string HashMD5(this string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            foreach (var t in hash) sb.Append(t.ToString("X2"));
            return sb.ToString();
        }

        public static string FirstCharacter(this string str)
        {
            return str.ToCharArray()[0].ToString();
        }

        public static string[] GetWords(this string phrase)
        {
            var pattern = new Regex(
                @"( [^\W_\d]              # starting with a letter
                                          # followed by a run of either...
                    ( [^\W_\d] |          #   more letters or
                      [-'\d](?=[^\W_\d])  #   ', -, or digit followed by a letter
                    )*
                    [^\W_\d]              # and finishing with a letter
                )",
                RegexOptions.IgnorePatternWhitespace);

            var input = phrase;
            List<string> x = (from Match m in pattern.Matches(input) select $"{m.Groups[1].Value}").ToList();
            return x.ToArray();
        }
    }
}
