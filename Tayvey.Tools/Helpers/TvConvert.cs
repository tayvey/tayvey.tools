using System;

namespace Tayvey.Tools.Helpers
{
    /// <summary>
    /// 转换
    /// </summary>
    public static class TvConvert
    {
        #region 字符串
        /// <summary>
        /// 字符串转INT
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int? TvToInt(this string? str)
        {
            if (int.TryParse(str?.Trim(), out var value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// 字符串转LONG
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long? TvToLong(this string? str)
        {
            if (long.TryParse(str?.Trim(), out var value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// 字符串转FLOAT
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float? TvToFloat(this string? str)
        {
            if (float.TryParse(str?.Trim(), out var value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// 字符串转DOUBLE
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double? TvToDouble(this string? str)
        {
            if (double.TryParse(str?.Trim(), out var value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// 字符串转DECIMAL
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal? TvToDecimal(this string? str)
        {
            if (decimal.TryParse(str?.Trim(), out var value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// 字符串转枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T? TvToEnum<T>(this string? str) where T : struct, Enum
        {
            if (Enum.TryParse<T>(str?.Trim(), out var value) && Enum.IsDefined(typeof(T), value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// 字符串转DATETIME
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime? TvToDateTime(this string? str, string? format = null)
        {
            if (!string.IsNullOrWhiteSpace(format) && DateTime.TryParseExact(str?.Trim(), format, null, System.Globalization.DateTimeStyles.None, out DateTime value))
            {
                return value;
            }

            if (string.IsNullOrWhiteSpace(format) && DateTime.TryParse(str?.Trim(), out value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// 字符串转TIMESPAN
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static TimeSpan? TvToTimeSpan(this string? str, string? format = null)
        {
            if (!string.IsNullOrWhiteSpace(format) && TimeSpan.TryParseExact(str?.Trim(), format, null, System.Globalization.TimeSpanStyles.None, out TimeSpan value))
            {
                return value;
            }

            if (string.IsNullOrWhiteSpace(format) && TimeSpan.TryParse(str?.Trim(), out value))
            {
                return value;
            }

            return null;
        }
        #endregion

        #region DATETIME
        /// <summary>
        /// DATETIME转字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string? TvToString(this DateTime? dt, string format = "yyyy-MM-dd HH:mm:ss") => dt?.ToString(format);

        /// <summary>
        /// DATETIME转字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string TvToString(this DateTime dt, string format = "yyyy-MM-dd HH:mm:ss") => dt.ToString(format);
        #endregion

        #region DECIMAL
        /// <summary>
        /// DECIMAL转字符串
        /// </summary>
        /// <param name="d"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string? TvToString(this decimal? d, string format = "0.########") => d?.ToString(format);

        /// <summary>
        /// DECIMAL转字符串
        /// </summary>
        /// <param name="d"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string TvToString(this decimal d, string format = "0.########") => d.ToString(format);
        #endregion
    }
}