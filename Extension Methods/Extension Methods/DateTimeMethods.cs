using System;
using System.Globalization;
using System.Text;

namespace Extension_Methods
{
    public static class DateTimeMethods
    {
        private static readonly NumberFormatInfo NumberFormatter = CultureInfo.InvariantCulture.NumberFormat;
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static readonly long TicksToUnixEpoch;
        private const long TicksPerMillisecond = 10000;

        static DateTimeMethods()
        {
            TicksToUnixEpoch = UnixEpoch.Ticks;
        }

        /// <summary> Creates a DateTime from a Utc Unix Timestamp. </summary>
        public static DateTime TryParseDateTime(long timestamp)
        {
            return UnixEpoch.AddSeconds(timestamp);
        }


        #region To Unix Time

        /// <summary> Converts a DateTime to Utc Unix Timestamp. </summary>
        public static long ToUnixTime(this DateTime date)
        {
            return (long)date.Subtract(UnixEpoch).TotalSeconds;
        }


        public static long ToUnixTimeLegacy(this DateTime date)
        {
            return (date.Ticks - TicksToUnixEpoch) / TicksPerMillisecond;
        }


        /// <summary> Converts a DateTime to a string containing the Utc Unix Timestamp.
        /// If the date equals DateTime.MinValue, returns an empty string. </summary>
        public static string ToUnixTimeString(this DateTime date)
        {
            return date == DateTime.MinValue ? "" : date.ToUnixTime().ToString(NumberFormatter);
        }


        /// <summary> Appends a Utc Unix Timestamp to the given StringBuilder.
        /// If the date equals DateTime.MinValue, nothing is appended. </summary>
        public static StringBuilder ToUnixTimeString(this DateTime date, StringBuilder sb)
        {
            if (date != DateTime.MinValue)
            {
                sb.Append(date.ToUnixTime());
            }
            return sb;
        }

        #endregion


        #region To Date Time

        /// <summary> Creates a DateTime from a Utc Unix Timestamp. </summary>
        public static DateTime ToDateTime(this long timestamp)
        {
            return UnixEpoch.AddSeconds(timestamp);
        }


        /// <summary> Tries to create a DateTime from a string containing a Utc Unix Timestamp.
        /// If the string was empty, returns false and does not affect result. </summary>
        public static bool ToDateTime(this string str, ref DateTime result)
        {
            if (str.Length <= 1 || !long.TryParse(str, out long t)) return false;
            result = UnixEpoch.AddSeconds(long.Parse(str));
            return true;
        }


        public static DateTime ToDateTimeLegacy(long timestamp)
        {
            return new DateTime(timestamp * TicksPerMillisecond + TicksToUnixEpoch, DateTimeKind.Utc);
        }


        public static bool ToDateTimeLegacy(this string str, ref DateTime result)
        {
            if (str.Length <= 1)
            {
                return false;
            }
            result = ToDateTimeLegacy(long.Parse(str));
            return true;
        }

        #endregion


        /// <summary> Converts a TimeSpan to a string containing the number of seconds.
        /// If the timestamp is zero seconds, returns an empty string. </summary>
        public static string ToTickString(this TimeSpan time)
        {
            return time == TimeSpan.Zero ? "" : (time.Ticks / TimeSpan.TicksPerSecond).ToString(NumberFormatter);
        }


        public static long ToSeconds(this TimeSpan time)
        {
            return (time.Ticks / TimeSpan.TicksPerSecond);
        }


        /// <summary> Tries to create a TimeSpan from a string containing the number of seconds.
        /// If the string was empty, returns false and sets result to TimeSpan.Zero </summary>
        public static bool ToTimeSpan(this string str, out TimeSpan result)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (str.Length == 0)
            {
                result = TimeSpan.Zero;
                return true;
            }

            if (long.TryParse(str, out var ticks))
            {
                result = new TimeSpan(ticks * TimeSpan.TicksPerSecond);
                return true;
            }

            result = TimeSpan.Zero;
            return false;
        }


        public static bool ToTimeSpanLegacy(this string str, ref TimeSpan result)
        {
            if (str.Length > 1)
            {
                result = new TimeSpan(long.Parse(str) * TicksPerMillisecond);
                return true;
            }

            return false;
        }


        #region MiniString

        public static StringBuilder ToTickString(this TimeSpan time, StringBuilder sb)
        {
            if (time != TimeSpan.Zero)
            {
                sb.Append(time.Ticks / TimeSpan.TicksPerSecond);
            }
            return sb;
        }


        public static string ToMiniString(this TimeSpan span)
        {
            if (span.TotalSeconds < 60)
            {
                return $"{span.Seconds}s";
            }

            if (span.TotalMinutes < 60)
            {
                return $"{span.Minutes}m{span.Seconds}s";
            }

            if (span.TotalHours < 48)
            {
                return $"{(int) Math.Floor(span.TotalHours)}h{span.Minutes}m";
            }

            return span.TotalDays < 15 
                ? $"{span.Days}d{span.Hours}h" 
                : $"{Math.Floor(span.TotalDays / 7):0}w{Math.Floor(span.TotalDays) % 7:0}d";
        }


        public static bool TryParseMiniTimespan(this string text, out TimeSpan result)
        {
            try
            {
                result = ParseMiniTimespan(text);
                return true;
            }
            catch (ArgumentException)
            {
            }
            catch (OverflowException)
            {
            }
            catch (FormatException) { }
            result = TimeSpan.Zero;
            return false;
        }


        public static readonly TimeSpan MaxTimeSpan = TimeSpan.FromDays(9999);


        public static TimeSpan ParseMiniTimespan(this string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            text = text.Trim();
            bool expectingDigit = true;
            TimeSpan result = TimeSpan.Zero;
            int digitOffset = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (expectingDigit)
                {
                    if (text[i] < '0' || text[i] > '9')
                    {
                        throw new FormatException();
                    }
                    expectingDigit = false;
                }
                else
                {
                    if (text[i] >= '0' && text[i] <= '9')
                    {
                        continue;
                    }

                    string numberString = text.Substring(digitOffset, i - digitOffset);
                    digitOffset = i + 1;
                    int number = int.Parse(numberString);
                    switch (char.ToLower(text[i]))
                    {
                        case 's':
                            result += TimeSpan.FromSeconds(number);
                            break;
                        case 'm':
                            result += TimeSpan.FromMinutes(number);
                            break;
                        case 'h':
                            result += TimeSpan.FromHours(number);
                            break;
                        case 'd':
                            result += TimeSpan.FromDays(number);
                            break;
                        case 'w':
                            result += TimeSpan.FromDays(number * 7);
                            break;
                        default:
                            throw new FormatException();
                    }
                }
            }
            return result;
        }

        #endregion


        #region CompactString

        public static string ToCompactString(this DateTime date)
        {
            return date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK");
        }


        public static string ToCompactString(this TimeSpan span)
        {
            return $"{span.Days}.{span.Hours:00}:{span.Minutes:00}:{span.Seconds:00}";
        }

        #endregion


        private static CultureInfo _cultureInfo = CultureInfo.CurrentCulture;

        /// <summary> Tries to parse a data in a culture-specific ways.</summary>
        /// <param name="dateString"> String to parse. </param>
        /// <param name="date"> Date to output. </param>
        /// <returns> True if date string could be parsed and was not empty/MinValue. </returns>
        public static bool TryParseLocalDate(string dateString, out DateTime date)
        {
            if (dateString == null) throw new ArgumentNullException(nameof(dateString));
            if (dateString.Length <= 1)
            {
                date = DateTime.MinValue;
                return false;
            }

            if (!DateTime.TryParse(dateString, _cultureInfo, DateTimeStyles.None, out date))
            {
                CultureInfo[] cultureList = CultureInfo.GetCultures(CultureTypes.FrameworkCultures);
                foreach (CultureInfo otherCultureInfo in cultureList)
                {
                    _cultureInfo = otherCultureInfo;
                    try
                    {
                        if (DateTime.TryParse(dateString, _cultureInfo, DateTimeStyles.None, out date))
                        {
                            return true;
                        }
                    }
                    catch (NotSupportedException) { }
                }
                throw new Exception("Could not find a culture that would be able to parse date/time formats.");
            }

            return true;
        }
    }
}
