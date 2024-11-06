using SqlSugar;

namespace Tayvey.Tools.Models
{
    /// <summary>
    /// SQLSUGAR连接配置
    /// </summary>
    public sealed class TvSqlSugarConnConfig
    {
        /// <summary>
        /// KEY
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DbType Type { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnStr { get; set; } = "";

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="key"></param>
        public TvSqlSugarConnConfig(string key)
        {
            Key = key;
        }
    }
}