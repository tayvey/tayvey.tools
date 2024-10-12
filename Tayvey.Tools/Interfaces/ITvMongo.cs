using MongoDB.Driver;

namespace Tayvey.Tools.Interfaces
{
    /// <summary>
    /// MONGODB接口
    /// </summary>
    public interface ITvMongo
    {
        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        MongoClient GetClient(string key);

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        IMongoDatabase GetDatabase(string key, string dbName);

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dbName"></param>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        IMongoCollection<T> GetCollection<T>(string key, string dbName, string collectionName) where T : class, new();

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IMongoCollection<T> GetCollection<T>() where T : class, new();
    }
}