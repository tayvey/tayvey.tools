#if NET6_0_OR_GREATER
#elif NETSTANDARD2_1
using System;
using System.Net;
#endif

namespace Tayvey.Tools
{
    /// <summary>
    /// Tv数据转换
    /// </summary>
    public static class TvDataConvert
    {
        #region IPEndPoint
        /// <summary>
        /// IP端点对象转换为IP地址+端口号元组对象
        /// </summary>
        /// <param name="endPoint">IP端点对象</param>
        /// <returns></returns>
        public static (string? ipAddress, ushort? port) TvtpToTuple(this IPEndPoint? endPoint)
        {
            try
            {
                if (endPoint == null)
                {
                    return (null, null);
                }

                var ipAddress = endPoint.Address.ToString();
                var port = (ushort)endPoint.Port;

                return (ipAddress, port);
            }
            catch
            {
                return (null, null);
            }
        }
        #endregion

        #region string
        /// <summary>
        /// 日期时间字符串转日期时间
        /// </summary>
        /// <param name="str">日期时间字符串</param>
        /// <param name="format">日期时间字符串格式</param>
        /// <returns></returns>
        public static DateTime? TvtpToDateTime(this string? str, string? format = null)
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
        /// int32字符串转int32
        /// </summary>
        /// <param name="str">int32字符串</param>
        /// <returns></returns>
        public static int? TvtpToInt32(this string? str)
        {
            if (int.TryParse(str?.Trim(), out var value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// 枚举项字符串转枚举项
        /// </summary>
        /// <typeparam name="T">枚举对象</typeparam>
        /// <param name="str">枚举项字符串</param>
        /// <returns></returns>
        public static T? TvtpToEnum<T>(this string? str) where T : struct, Enum
        {
#if NET6_0_OR_GREATER
            if (Enum.TryParse<T>(str?.Trim(), out var value) && Enum.IsDefined(value))
#elif NETSTANDARD2_1
            if (Enum.TryParse<T>(str?.Trim(), out var value) && Enum.IsDefined(typeof(T), value))
#endif
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// float字符串转float
        /// </summary>
        /// <param name="str">float字符串</param>
        /// <returns></returns>
        public static float? TvtpToFloat(this string? str)
        {
            if (float.TryParse(str?.Trim(), out var value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// decimal字符串转decimal
        /// </summary>
        /// <param name="str">decimal字符串</param>
        /// <returns></returns>
        public static decimal? TvtpToDecimal(this string? str)
        {
            if (decimal.TryParse(str?.Trim(), out var value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// TimeSpan字符串转TimeSpan
        /// </summary>
        /// <param name="str">TimeSpan字符串</param>
        /// <param name="format">TimeSpan字符串格式</param>
        /// <returns></returns>
        public static TimeSpan? TvtpToTimeSpan(this string? str, string? format = null)
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

        #region DateTime
        /// <summary>
        /// 日期时间转日期时间字符串
        /// </summary>
        /// <param name="dt">日期时间</param>
        /// <param name="format">转换格式</param>
        /// <returns></returns>
        public static string? TvtpToString(this DateTime? dt, string format = "yyyy-MM-dd HH:mm:ss") => dt?.ToString(format);

        /// <summary>
        /// 日期时间转日期时间字符串
        /// </summary>
        /// <param name="dt">日期时间</param>
        /// <param name="format">转换格式</param>
        /// <returns></returns>
        public static string TvtpToString(this DateTime dt, string format = "yyyy-MM-dd HH:mm:ss") => dt.ToString(format);
        #endregion

        #region decimal
        /// <summary>
        /// decimal转decimal字符串
        /// </summary>
        /// <param name="d"></param>
        /// <param name="format">转换格式</param>
        /// <returns></returns>
        public static string? TvtpToString(this decimal? d, string format = "0.########") => d?.ToString(format);

        /// <summary>
        /// decimal转decimal字符串
        /// </summary>
        /// <param name="d"></param>
        /// <param name="format">转换格式</param>
        /// <returns></returns>
        public static string TvtpToString(this decimal d, string format = "0.########") => d.ToString(format);
        #endregion

        #region Enum
        /// <summary>
        /// 枚举项转枚举项索引字符串
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="enumItem">枚举项</param>
        /// <returns></returns>
        public static string? TvtpToString<T>(this T? enumItem) where T : struct, Enum => enumItem.GetHashCode().ToString();

        /// <summary>
        /// 枚举项转枚举项索引字符串
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="enumItem">枚举项</param>
        /// <returns></returns>
        public static string TvtpToString<T>(this T enumItem) where T : struct, Enum => enumItem.GetHashCode().ToString();
        #endregion
    }
}