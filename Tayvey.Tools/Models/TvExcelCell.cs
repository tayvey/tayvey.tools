namespace Tayvey.Tools.Models
{
    /// <summary>
    /// EXCEL单元格信息
    /// </summary>
    public sealed class TvExcelCell
    {
        /// <summary>
        /// 工作表索引
        /// </summary>
        public int Worksheet { get; }

        /// <summary>
        /// 工作表名称
        /// </summary>
        public string WorksheetName { get; }

        /// <summary>
        /// 行索引
        /// </summary>
        public int Row { get; }

        /// <summary>
        /// 列索引
        /// </summary>
        public int Col { get; }

        /// <summary>
        /// 单元格内容字符串
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="worksheet">工作表索引</param>
        /// <param name="worksheetName">工作表名称</param>
        /// <param name="row">行索引</param>
        /// <param name="col">列索引</param>
        /// <param name="value">单元格内容字符串</param>
        internal TvExcelCell(int worksheet, string worksheetName, int row, int col, string value)
        {
            Worksheet = worksheet;
            WorksheetName = worksheetName;
            Row = row;
            Col = col;
            Value = value;
        }
    }
}