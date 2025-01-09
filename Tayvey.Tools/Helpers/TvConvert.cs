using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using OfficeOpenXml;

namespace Tayvey.Tools
{
    /// <summary>
    /// 转换
    /// </summary>
    public static class TvConvert
    {
        /// <summary>
        /// 静态构造
        /// </summary>
        static TvConvert()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // EPPlus非商业用途
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // 注册字符编码
        }

        #region 字符串
        /// <summary>
        /// 字符串转byte
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte? TvToByte(this string? str)
        {
            if (byte.TryParse(str?.Trim(), out var value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// 字符串转short
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static short? TvToShort(this string? str)
        {
            if (byte.TryParse(str?.Trim(), out var value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// 字符串转int
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
        /// 字符串转long
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
        /// 字符串转float
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
        /// 字符串转double
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
        /// 字符串转decimal
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
        /// 字符串转enum
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
        /// 字符串转DateTime
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
        /// 字符串转TimeSpan
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

        /// <summary>
        /// XML字符串转实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T? TvToXmlEntity<T>(this string str) where T : class
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using var reader = new StringReader(str);
                return (T?)serializer.Deserialize(reader);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region DateTime
        /// <summary>
        /// DateTime转字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string? TvToString(this DateTime? dt, string format = "yyyy-MM-dd HH:mm:ss") => dt?.ToString(format);

        /// <summary>
        /// DateTime转字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string TvToString(this DateTime dt, string format = "yyyy-MM-dd HH:mm:ss") => dt.ToString(format);
        #endregion

        #region float
        /// <summary>
        /// float转字符串
        /// </summary>
        /// <param name="f"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string? TvToString(this float? f, string format = "0.########") => f?.ToString(format);

        /// <summary>
        /// float转字符串
        /// </summary>
        /// <param name="f"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string TvToString(this float f, string format = "0.########") => f.ToString(format);
        #endregion

        #region double
        /// <summary>
        /// double转字符串
        /// </summary>
        /// <param name="d"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string? TvToString(this double? d, string format = "0.########") => d?.ToString(format);

        /// <summary>
        /// double转字符串
        /// </summary>
        /// <param name="d"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string TvToString(this double d, string format = "0.########") => d.ToString(format);
        #endregion

        #region decimal
        /// <summary>
        /// decimal转字符串
        /// </summary>
        /// <param name="d"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string? TvToString(this decimal? d, string format = "0.########") => d?.ToString(format);

        /// <summary>
        /// decimal转字符串
        /// </summary>
        /// <param name="d"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string TvToString(this decimal d, string format = "0.########") => d.ToString(format);
        #endregion

        #region entity
        /// <summary>
        /// 实体转XML字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string? TvToXmlString<T>(this T entity) where T : class, new()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using var writer = new StringWriter();
                var settings = new XmlWriterSettings
                {
                    OmitXmlDeclaration = true,
                    Indent = false
                };
                using var xmlWriter = XmlWriter.Create(writer, settings);
                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);
                serializer.Serialize(xmlWriter, entity, namespaces);
                return writer.ToString();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 实体转MongoDB upsert对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static UpdateDefinition<T>? TvToMongoUpsert<T>(this T entity, HashSet<string>? setOnInsertFields = null)
            where T : class, new()
        {
            setOnInsertFields ??= new HashSet<string>();

            var bson = entity.ToBsonDocument();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            UpdateDefinition<T>? upsert = null;
            foreach (var item in bson)
            {
                var name = item.Name;
                var value = item.Value;

                var prop = props.FirstOrDefault(x => x.Name == name || x.GetCustomAttribute<BsonElementAttribute>()?.ElementName == name);
                if (prop == null)
                {
                    continue;
                }

                var isUpsert = setOnInsertFields.Contains(name) || setOnInsertFields.Contains(prop.Name);

                upsert = isUpsert switch
                {
                    true when upsert == null => Builders<T>.Update.SetOnInsert(name, value),
                    true when upsert != null => upsert.SetOnInsert(name, value),
                    false when upsert == null => Builders<T>.Update.Set(name, value),
                    _ => upsert.Set(name, value)
                };
            }

            return upsert;
        }
        #endregion

        #region stream
        /// <summary>
        /// 读取为excel单元格对象
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <returns></returns>
        public static List<TvExcelCell> TvToExcelCell(this Stream stream)
        {
            using var package = new ExcelPackage(stream); // 流读取EXCEL

            // 单元格集合
            var cells = new ConcurrentBag<TvExcelCell>();

            // 遍历读取EXCEL每个有效的工作表
            foreach (var worksheet in package.Workbook.Worksheets.Where(i => i.Dimension != null))
            {
                // 并行遍历单元格
                Parallel.ForEach(worksheet.Cells, cell =>
                {
                    var col = cell.Start.Column; // 列号
                    var row = cell.Start.Row; // 行号
                    var value = cell.Value?.ToString(); // 内容

                    // 过滤单元格
                    if (value == null)
                    {
                        return;
                    }

                    // 写入集合
                    cells.Add(new TvExcelCell(worksheet.Index + 1, worksheet.Name, row, col, value));
                });
            }

            return cells.ToList();
        }
        #endregion
    }
}