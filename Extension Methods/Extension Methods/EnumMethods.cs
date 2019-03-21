using System;
using System.Reflection;

namespace Extension_Methods
{
    public class EnumText : Attribute
    {
        public EnumText(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
    }

    public static class EnumMethods
    {
        public static string ToString(this Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo.Length <= 0) return en.ToString();
            object[] attrs = memInfo[0].GetCustomAttributes(
                typeof(EnumText),
                false);
            return attrs.Length > 0 ? ((EnumText)attrs[0]).Text : en.ToString();
        }

        public static bool TryParse<TEnum>(string value, out TEnum output, bool ignoreCase)
        {
            if (value == null) throw new ArgumentNullException("value");
            try
            {
                output = (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
                return Enum.IsDefined(typeof(TEnum), output);
            }
            catch (ArgumentException)
            {
                output = default(TEnum);
                return false;
            }
        }
    }
}
