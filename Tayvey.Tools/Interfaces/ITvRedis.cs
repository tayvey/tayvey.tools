using StackExchange.Redis;

namespace Tayvey.Tools
{
    /// <summary>
    /// Redis仓储接口
    /// </summary>
    public interface ITvRedis
    {
        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IConnectionMultiplexer GetClient(string key);

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="key"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public IDatabase GetDatabase(string key, int db = -1);
    }
}