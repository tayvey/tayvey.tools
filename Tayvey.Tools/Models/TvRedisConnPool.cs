using StackExchange.Redis;

namespace Tayvey.Tools.Models
{
    /// <summary>
    /// REDIS连接池
    /// </summary>
    internal sealed class TvRedisConnPool
    {
        /// <summary>
        /// KEY
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 连接
        /// </summary>
        public ConnectionMultiplexer Client { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="key"></param>
        /// <param name="client"></param>
        public TvRedisConnPool(string key, ConnectionMultiplexer client)
        {
            Key = key;
            Client = client;
        }
    }
}