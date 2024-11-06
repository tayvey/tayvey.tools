using SqlSugar;

namespace Tayvey.Tools.Models
{
    /// <summary>
    /// SQLSUGAR连接池
    /// </summary>
    internal sealed class TvSqlSugarConnPool
    {
        /// <summary>
        /// KEY
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 连接
        /// </summary>
        public SqlSugarScope Client { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="key"></param>
        /// <param name="client"></param>
        public TvSqlSugarConnPool(string key, SqlSugarScope client)
        {
            Key = key;
            Client = client;
        }
    }
}