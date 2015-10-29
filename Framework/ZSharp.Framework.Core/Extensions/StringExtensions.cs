using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;
using ZSharp.Framework.Core;

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
        public static DateTime? ToDateTimeOrNull(this string str, DateTime? defaultValue = null)
        {
            DateTime d;
            if (DateTime.TryParse(str, out d))
                return d;
            else
            {
                if (DateTime.TryParseExact(str, new string[] { "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss", "yyyyMMdd", "yyyyMMdd HH:mm:ss", "yyyy/MM/dd", "yyyy'/'MM'/'dd HH:mm:ss", "MM'/'dd'/'yyyy HH:mm:ss", "yyyy-M-d", "yyy-M-d hh:mm:ss" }, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out d))
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
        public static DateTime? ToDateTimeOrNull(this string str, string dateFmt, DateTime? defaultValue = null)
        {
            DateTime d;
            //if (DateTime.TryParse(str, out d))
            //    return d;
            //else {
            if (DateTime.TryParseExact(str, dateFmt, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out d))
                return d;
            else
                return defaultValue;
            //}        
        }

        private static readonly DateTime MinDate = new DateTime(1900, 1, 1);

        /// <summary>
        /// 轉換日期，轉換失敗時，返回 defaultValue
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string str, DateTime defaultValue = default(DateTime))
        {
            DateTime d;
            if (DateTime.TryParse(str, out d))
                return d;
            else
            {
                if (DateTime.TryParseExact(str, new string[] { "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss", "yyyyMMdd", "yyyyMMdd HH:mm:ss", "yyyy/MM/dd", "yyyy/MM/dd HH:mm:ss", "MM/dd/yyyy HH:mm:ss" }, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out d))
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
        public static DateTime ToDateTime(this string str, string dateFmt, DateTime defaultValue)
        {
            DateTime d;
            //if (DateTime.TryParse(str, out d))
            //    return d;
            //else {
            if (DateTime.TryParseExact(str, dateFmt, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out d))
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
        public static DateTime? ToDateTimeOrNull(this string str)
        {
            return str.ToDateTimeOrNull(null);
        }

        /// <summary>
        /// 轉換日期，轉換失敗時，返回當前時間
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string str)
        {
            return str.ToDateTime(DateTime.Now);
        }

        /// <summary>
        /// 是否為日期型字串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string str)
        {
            //return Regex.IsMatch(str, @"^(((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$ ");
            DateTime d;
            if (DateTime.TryParseExact(str, new string[] { "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss", "yyyyMMdd", "yyyyMMdd HH:mm:ss", "yyyy/MM/dd", "yyyy/MM/dd HH:mm:ss", "MM/dd/yyyy", "MM/dd/yyyy HH:mm:ss" }, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out d))
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

        /// <summary>
        /// Converts string to a Url object and appends a segment to the URL path, 
        /// ensuring there is one and only one '/' character as a seperator.
        /// </summary>
        /// <param name="segment">The segment to append</param>
        /// <returns>the resulting Url object</returns>
        public static Url AppendPathSegment(this string url, string segment)
        {
            return new Url(url).AppendPathSegment(segment);
        }

        /// <summary>
        /// Appends multiple segments to the URL path, ensuring there is one and only one '/' character as a seperator.
        /// </summary>
        /// <param name="segments">The segments to append</param>
        /// <returns>the Url object with the segments appended</returns>
        public static Url AppendPathSegments(this string url, params string[] segments)
        {
            return new Url(url).AppendPathSegments(segments);
        }

        /// <summary>
        /// Appends multiple segments to the URL path, ensuring there is one and only one '/' character as a seperator.
        /// </summary>
        /// <param name="segments">The segments to append</param>
        /// <returns>the Url object with the segments appended</returns>
        public static Url AppendPathSegments(this string url, IEnumerable<string> segments)
        {
            return new Url(url).AppendPathSegments(segments);
        }

        /// <summary>
        /// Converts string to a Url object and adds a parameter to the query string, overwriting the value if name exists.
        /// </summary>
        /// <param name="name">name of query string parameter</param>
        /// <param name="value">value of query string parameter</param>
        /// <returns>The Url obect with the query string parameter added</returns>
        public static Url SetQueryParam(this string url, string name, object value)
        {
            return new Url(url).SetQueryParam(name, value);
        }

        /// <summary>
        /// Converts string to a Url object, parses values object into name/value pairs, and adds them to the query string,
        /// overwriting any that already exist.
        /// </summary>
        /// <param name="values">Typically an anonymous object, ie: new { x = 1, y = 2 }</param>
        /// <returns>The Url object with the query string parameters added</returns>
        public static Url SetQueryParams(this string url, object values)
        {
            return new Url(url).SetQueryParams(values);
        }

        /// <summary>
        /// Converts string to a Url object and removes a name/value pair from the query string by name.
        /// </summary>
        /// <param name="name">Query string parameter name to remove</param>
        /// <returns>The Url object with the query string parameter removed</returns>
        public static Url RemoveQueryParam(this string url, string name)
        {
            return new Url(url).RemoveQueryParam(name);
        }

        /// <summary>
        /// Converts string to a Url object and removes multiple name/value pairs from the query string by name.
        /// </summary>
        /// <param name="names">Query string parameter names to remove</param>
        /// <returns>The Url object with the query string parameters removed</returns>
        public static Url RemoveQueryParams(this string url, params string[] names)
        {
            return new Url(url).RemoveQueryParams(names);
        }

        /// <summary>
        /// Converts string to a Url object and removes multiple name/value pairs from the query string by name.
        /// </summary>
        /// <param name="names">Query string parameter names to remove</param>
        /// <returns>The Url object with the query string parameters removed</returns>
        public static Url RemoveQueryParams(this string url, IEnumerable<string> names)
        {
            return new Url(url).RemoveQueryParams(names);
        }

        /// <summary>
        /// Trims the URL to its root, including the scheme, any user info, host, and port (if specified).
        /// </summary>
        /// <returns>A Url object.</returns>
        public static Url ResetToRoot(this string url)
        {
            return new Url(url).ResetToRoot();
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
    }
}

