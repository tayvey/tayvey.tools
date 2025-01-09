using SqlSugar;

namespace Tayvey.Tools
{
    /// <summary>
    /// 关系型数据库连接配置
    /// </summary>
    public sealed class TvSqlConnectionConfig
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DbType Type { get; set; }

        /// <summary>
        /// 关键词
        /// </summary>
        public string Key { get; set; } = "";

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionStr { get; set; } = "";
    }
}