using MongoDB.Driver;

namespace Tayvey.Tools
{
    /// <summary>
    /// MongoDB连接池
    /// </summary>
    internal sealed class TvMongoConnectionPool
    {
        /// <summary>
        /// 连接配置
        /// </summary>
        public TvMongoConnectionConfig Config { get; set; }

        /// <summary>
        /// 连接客户端
        /// </summary>
        public MongoClient Client { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="config"></param>
        /// <param name="client"></param>
        public TvMongoConnectionPool(TvMongoConnectionConfig config, MongoClient client)
        {
            Config = config;
            Client = client;
        }
    }
}