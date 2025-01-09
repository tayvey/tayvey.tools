using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tayvey.Tools
{
    /// <summary>
    /// MongoDB仓储
    /// </summary>
    public class TvMongo : ITvMongo
    {
        /// <summary>
        /// 连接池
        /// </summary>
        private readonly List<TvMongoConnectionPool> _clients;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="configs"></param>
        public TvMongo(params TvMongoConnectionConfig[] configs)
        {
            // 创建连接
            _clients = configs.Select(config =>
            {
                // 建立连接
                var client = new MongoClient(config.ConnectionStr);
                return new TvMongoConnectionPool(config, client);
            }).ToList();
        }

        /// <summary>
        /// 获取数据库集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dbName"></param>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public IMongoCollection<T> GetCollection<T>(string key, string dbName, string collectionName) where T : class, new()
        {
            var db = GetDatabase(key, dbName);
            return db.GetCollection<T>(collectionName);
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IMongoDatabase GetDatabase(string key, string dbName)
        {
            var db = _clients.FirstOrDefault(i => i.Config.Key == key)
                ?? throw new Exception($"数据库连接[{key}]不存在");

            return db.Client.GetDatabase(dbName);
        }
    }
}