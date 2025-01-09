using SqlSugar;

namespace Tayvey.Tools
{
    /// <summary>
    /// 关系型数据库连接池
    /// </summary>
    internal sealed class TvSqlConnectionPool
    {
        /// <summary>
        /// 连接配置
        /// </summary>
        public TvSqlConnectionConfig Config { get; set; }

        /// <summary>
        /// 连接客户端
        /// </summary>
        public SqlSugarScope Client { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="config"></param>
        /// <param name="client"></param>
        public TvSqlConnectionPool(TvSqlConnectionConfig config, SqlSugarScope client)
        {
            Config = config;
            Client = client;
        }
    }
}