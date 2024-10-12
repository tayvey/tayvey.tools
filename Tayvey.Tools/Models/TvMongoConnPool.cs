using MongoDB.Driver;

namespace Tayvey.Tools.Models
{
    /// <summary>
    /// MONGODB连接池
    /// </summary>
    internal sealed class TvMongoConnPool
    {
        /// <summary>
        /// KEY
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 连接
        /// </summary>
        public MongoClient Client { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="key"></param>
        /// <param name="client"></param>
        public TvMongoConnPool(string key, MongoClient client)
        {
            Key = key;
            Client = client;
        }
    }
}