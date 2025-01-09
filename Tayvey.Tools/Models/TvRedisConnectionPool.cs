using StackExchange.Redis;

namespace Tayvey.Tools
{
    /// <summary>
    /// Redis连接池
    /// </summary>
    internal sealed class TvRedisConnectionPool
    {
        /// <summary>
        /// 连接配置
        /// </summary>
        public TvRedisConnectionConfig Config { get; set; }

        /// <summary>
        /// 连接客户端
        /// </summary>
        public ConnectionMultiplexer Client { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="config"></param>
        /// <param name="client"></param>
        public TvRedisConnectionPool(TvRedisConnectionConfig config, ConnectionMultiplexer client)
        {
            Config = config;
            Client = client;
        }
    }
}