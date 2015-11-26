using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.Extensions
{
    public static class StringExtensions
    {
        #region To int

        public static int ToInt(this string str, int defaultValue)
        {
            int v;
            if (int.TryParse(str, out v))
            {
                return v;
            }
            else
                return defaultValue;
        }


        public static int ToInt(this string str)
        {
            return str.ToInt(0);
        }

        #endregion

        #region To Bool
        public static bool ToBool(this string str, bool defaultValue)
        {
            bool b;
            if (bool.TryParse(str, out b))
            {
                return b;
            }
            else
            {
                return defaultValue;
            }
        }

        #endregion

        #region To Long

        public static long ToLong(this string str, long defaultValue)
        {
            long v;
            if (long.TryParse(str, out v))
                return v;
            else
                return defaultValue;
        }

        public static long ToLong(this string str)
        {
            return str.ToLong(0);
        }

        #endregion

        #region To byte

        public static byte ToByte(this string str, byte defaultValue)
        {
            byte v;
            if (byte.TryParse(str, out v))
            {
                return v;
            }
            else
                return defaultValue;
        }

        public static byte ToByte(this string str)
        {
            return str.ToByte(0);
        }

        #endregion

        #region Url

        /// <summary>
        /// 修正URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string FixUrl(this string url)
        {
            return url.FixUrl("");
        }

        /// <summary>
        /// 修正URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="defaultPrefix"></param>
        /// <returns></returns>
        public static string FixUrl(this string url, string defaultPrefix)
        {
            // 必須這樣,請不要修改
            if (url == null)
                url = "";

            if (defaultPrefix == null)
                defaultPrefix = "";
            string tmp = url.Trim();
            if (!Regex.Match(tmp, "^(http|https):").Success)
            {
                tmp = string.Format("{0}/{1}", defaultPrefix, tmp);
            }
            tmp = Regex.Replace(tmp, @"(?<!(http|https):)[\\/]+", "/").Trim();
            return tmp;
        }

        #endregion

        #region DateTime
        /// <summary>
        /// 轉換為日期，如果轉換失敗，返回預設值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTimeOffset? ToDateTimeOrNull(this string str, DateTimeOffset? defaultValue = null)
        {
            DateTimeOffset d;
            if (DateTimeOffset.TryParse(str, out d))
                return d;
            else
            {
                if (DateTimeOffset.TryParseExact(str, new string[] { "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss", "yyyyMMdd", "yyyyMMdd HH:mm:ss", "yyyy/MM/dd", "yyyy'/'MM'/'dd HH:mm:ss", "MM'/'dd'/'yyyy HH:mm:ss", "yyyy-M-d", "yyy-M-d hh:mm:ss" }, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out d))
                    return d;
                else
                    return defaultValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="dateFmt"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTimeOffset? ToDateTimeOrNull(this string str, string dateFmt, DateTimeOffset? defaultValue = null)
        {
            DateTimeOffset d;
            //if (DateTimeOffset.TryParse(str, out d))
            //    return d;
            //else {
            if (DateTimeOffset.TryParseExact(str, dateFmt, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out d))
                return d;
            else
                return defaultValue;
            //}        
        }

        private static readonly DateTimeOffset MinDate = new DateTime(1900, 1, 1);

        /// <summary>
        /// 轉換日期，轉換失敗時，返回 defaultValue
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTimeOffset ToDateTime(this string str, DateTimeOffset defaultValue = default(DateTimeOffset))
        {
            DateTimeOffset d;
            if (DateTimeOffset.TryParse(str, out d))
                return d;
            else
            {
                if (DateTimeOffset.TryParseExact(str, new string[] { "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss", "yyyyMMdd", "yyyyMMdd HH:mm:ss", "yyyy/MM/dd", "yyyy/MM/dd HH:mm:ss", "MM/dd/yyyy HH:mm:ss" }, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out d))
                    return d;
                else
                    if (default(DateTime) == defaultValue)
                        return MinDate;
                    else
                        return defaultValue;
            }
        }

        /// <summary>
        /// 按給定日期格式進行日期轉換
        /// </summary>
        /// <param name="str"></param>
        /// <param name="dateFmt"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTimeOffset ToDateTime(this string str, string dateFmt, DateTimeOffset defaultValue)
        {
            DateTimeOffset d;
            //if (DateTimeOffset.TryParse(str, out d))
            //    return d;
            //else {
            if (DateTimeOffset.TryParseExact(str, dateFmt, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out d))
                return d;
            else
                return defaultValue;
            //}            
        }

        /// <summary>
        /// 轉換為日期，轉換失敗時，返回null
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTimeOffset? ToDateTimeOrNull(this string str)
        {
            return str.ToDateTimeOrNull(null);
        }

        /// <summary>
        /// 轉換日期，轉換失敗時，返回當前時間
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTimeOffset ToDateTime(this string str)
        {
            return str.ToDateTime(DateTimeOffset.Now);
        }

        /// <summary>
        /// 是否為日期型字串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string str)
        {
            //return Regex.IsMatch(str, @"^(((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$ ");
            DateTimeOffset d;
            if (DateTimeOffset.TryParseExact(str, new string[] { "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss", "yyyyMMdd", "yyyyMMdd HH:mm:ss", "yyyy/MM/dd", "yyyy/MM/dd HH:mm:ss", "MM/dd/yyyy", "MM/dd/yyyy HH:mm:ss" }, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out d))
                return true;
            else
                return false;
        }
        #endregion

        public const string CarriageReturnLineFeed = "\r\n";
        public const string Empty = "";
        public const char CarriageReturn = '\r';
        public const char LineFeed = '\n';
        public const char Tab = '\t';

        private delegate void ActionLine(TextWriter textWriter, string line);

        [DebuggerStepThrough]
        public static string FormatInvariant(this string format, params object[] objects)
        {
            return string.Format(CultureInfo.InvariantCulture, format, objects);
        }

        /// <summary>
        /// Determines whether the string is null, empty or all whitespace.
        /// </summary>
        [DebuggerStepThrough]
        public static bool IsEmpty(this string value)
        {
            if (value == null || value.Length == 0)
                return true;

            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i]))
                    return false;
            }

            return true;
        }

        [DebuggerStepThrough]
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        #region
        /// <summary>
        /// Checks if a string is a well-formed URL.
        /// </summary>
        /// <param name="s">The string to check</param>
        /// <returns>true if s is a well-formed URL</returns>
        public static bool IsUrl(this string s)
        {
            return s != null && Uri.IsWellFormedUriString(s, UriKind.Absolute);
        }        
        #endregion

        #region 处理t-sql中插入的值，过滤特殊字符

        /// <summary>
        /// 处理t-sql中插入的值，防止意外字符导致错误
        /// </summary>
        /// <param name="str">需要插入的参数值</param>
        /// <returns></returns>
        public static string ToSecuritySQL(this string str)
        {
            return str.Replace("'", "''").Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]").Replace("^", "[^]");
        }

        #endregion

        [DebuggerStepThrough]
        public static string FormatWith(this string format, params object[] args)
        {
            return FormatWith(format, CultureInfo.CurrentCulture, args);
        }

        /// <summary>
        /// Formats a string to the current culture.
        /// </summary>
        /// <param name="formatString">The format string.</param>
        /// <param name="objects">The objects.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string FormatCurrent(this string format, params object[] objects)
        {
            return string.Format(CultureInfo.CurrentCulture, format, objects);
        }

        /// <summary>Debug.WriteLine</summary>
        [DebuggerStepThrough]
        public static void Dump(this string value, bool appendMarks = false)
        {
            Debug.WriteLine(value);
            Debug.WriteLineIf(appendMarks, "------------------------------------------------");
        }

        [DebuggerStepThrough]
        public static string NullEmpty(this string value)
        {
            return (string.IsNullOrEmpty(value)) ? null : value;
        }

        [DebuggerStepThrough]
        public static string EmptyNull(this string value)
        {
            return (value ?? string.Empty).Trim();
        }

        /// <summary>
        /// Ensures the target string ends with the specified string.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="value">The value.</param>
        /// <returns>The target string with the value string at the end.</returns>
        [DebuggerStepThrough]
        public static string EnsureEndsWith(this string value, string endWith)
        {
            GuardHelper.ArgumentNotNull(value, "value");
            GuardHelper.ArgumentNotNull(endWith, "endWith");

            if (value.Length >= endWith.Length)
            {
                if (string.Compare(value, value.Length - endWith.Length, endWith, 0, endWith.Length, StringComparison.OrdinalIgnoreCase) == 0)
                    return value;

                string trimmedString = value.TrimEnd(null);

                if (string.Compare(trimmedString, trimmedString.Length - endWith.Length, endWith, 0, endWith.Length, StringComparison.OrdinalIgnoreCase) == 0)
                    return value;
            }

            return value + endWith;
        }

        /// <summary>
        /// Determines whether this instance and another specified System.String object have the same value.
        /// </summary>
        /// <param name="instance">The string to check equality.</param>
        /// <param name="comparing">The comparing with string.</param>
        /// <returns>
        /// <c>true</c> if the value of the comparing parameter is the same as this string; otherwise, <c>false</c>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsCaseInsensitiveEqual(this string value, string comparing)
        {
            return string.Compare(value, comparing, StringComparison.OrdinalIgnoreCase) == 0;
        }

        [DebuggerStepThrough]
        public static bool IsMatch(this string input, string pattern, RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline)
        {
            return Regex.IsMatch(input, pattern, options);
        }
    }
}

