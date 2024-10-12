using OfficeOpenXml;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tayvey.Tools.Models;

namespace Tayvey.Tools.Extension
{
    /// <summary>
    /// EXCEL文件操作扩展
    /// </summary>
    public static class TvExcelEx
    {
        /// <summary>
        /// 静态构造
        /// </summary>
        static TvExcelEx()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // EPPlus非商业用途
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // 注册字符编码
        }

        /// <summary>
        /// 读取为EXCEL单元格对象
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <returns></returns>
        public static List<TvExcelCell> ReadToExcelCell(this Stream stream)
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
    }
}