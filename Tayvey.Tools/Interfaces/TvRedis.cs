using StackExchange.Redis;

namespace Tayvey.Tools.Interfaces
{
    /// <summary>
    /// REDIS接口
    /// </summary>
    public interface ITvRedis
    {
        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ConnectionMultiplexer GetClient(string key);

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="key"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        IDatabase GetDatabase(string key, int db = -1);
    }
}