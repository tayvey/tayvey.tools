//namespace Tayvey.Tools.TvExcels.Models
//{
//    /// <summary>
//    /// TvExcel单元格信息
//    /// </summary>
//    public sealed class TvExcelCell
//    {
//        /// <summary>
//        /// 工作表索引
//        /// </summary>
//#if NET6_0_OR_GREATER
//        public uint Worksheet { get; init; }
//#else
//        public uint Worksheet { get; }
//#endif

//        /// <summary>
//        /// 工作表名称
//        /// </summary>
//#if NET6_0_OR_GREATER
//        public string WorksheetName { get; init; } = "";
//#else
//        public string WorksheetName { get; } = "";
//#endif

//        /// <summary>
//        /// 行索引
//        /// </summary>
//#if NET6_0_OR_GREATER
//        public uint Row { get; init; }
//#else
//        public uint Row { get; }
//#endif

//        /// <summary>
//        /// 列索引
//        /// </summary>
//#if NET6_0_OR_GREATER
//        public uint Col { get; init; }
//#else
//        public uint Col { get; }
//#endif

//        /// <summary>
//        /// 单元格内容字符串
//        /// </summary>
//#if NET6_0_OR_GREATER
//        public string Value { get; init; } = "";
//#else
//        public string Value { get; } = "";
//#endif

//#if NETSTANDARD2_1
//        /// <summary>
//        /// 初始化
//        /// </summary>
//        /// <param name="worksheet">工作表索引</param>
//        /// <param name="worksheetName">工作表名称</param>
//        /// <param name="row">行索引</param>
//        /// <param name="col">列索引</param>
//        /// <param name="value">单元格内容字符串</param>
//        public TvExcelCell(
//            uint worksheet,
//            string worksheetName,
//            uint row,
//            uint col,
//            string value
//        )
//        {
//            Worksheet = worksheet;
//            WorksheetName = worksheetName;
//            Row = row;
//            Col = col;
//            Value = value;
//        }
//#endif
//    }
//}