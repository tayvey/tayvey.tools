using MongoDB.Driver;

namespace Tayvey.Tools
{
    /// <summary>
    /// MongoDB仓储接口
    /// </summary>
    public interface ITvMongo
    {
        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public IMongoDatabase GetDatabase(string key, string dbName);

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IMongoDatabase GetDatabase(string key) => GetDatabase(key, key);

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dbName"></param>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public IMongoCollection<T> GetCollection<T>(string key, string dbName, string collectionName) where T : class, new();

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public IMongoCollection<T> GetCollection<T>(string key, string collectionName) where T : class, new() => GetCollection<T>(key, key, collectionName);
    }
}