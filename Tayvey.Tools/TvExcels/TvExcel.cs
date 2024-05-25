#if NETSTANDARD2_1
using OfficeOpenXml;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tayvey.Tools.TvExcels.Models;
#endif

namespace Tayvey.Tools.TvExcels
{
    /// <summary>
    /// TvExcel
    /// </summary>
    public static class TvExcel
    {
        /// <summary>
        /// 读取Excel
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <returns></returns>
        public static async Task<List<TvExcelCell>> ReadAsync(this Stream stream)
        {
            try
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial; // EPPlus非商业用途
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // 注册字符编码
                using var package = new ExcelPackage(stream); // 流读取Excel

                // 单元格集合
                var cells = new ConcurrentBag<TvExcelCell>();

                // 遍历读取Excel每个有效的工作表
                foreach (var worksheet in package.Workbook.Worksheets.Where(i => i.Dimension != null))
                {
                    // 异步遍历单元格
                    await worksheet.Cells.TvForEachAsync(cell =>
                    {
                        var col = cell.Start.Column; // 列号
                        var row = cell.Start.Row; // 行号
                        var value = cell.Value?.ToString(); // 内容

                        // 过滤单元格
                        if (value == null)
                        {
                            return;
                        }

                        // 写入到集合
#if NET6_0_OR_GREATER
                        cells.Add(new TvExcelCell
                        {
                            Worksheet = (uint)worksheet.Index + 1,
                            WorksheetName = worksheet.Name,
                            Row = (uint)row,
                            Col = (uint)col,
                            Value = value
                        });
#else
                        cells.Add(new TvExcelCell((uint)worksheet.Index + 1, worksheet.Name, (uint)row, (uint)col, value));
#endif
                    });
                }

#if NET8_0_OR_GREATER
            return [.. cells];
#else
                return cells.ToList();
#endif
            }
            catch
            {
#if NET8_0_OR_GREATER
            return [];
#else
                return new List<TvExcelCell>();
#endif
            }
        }
    }
}