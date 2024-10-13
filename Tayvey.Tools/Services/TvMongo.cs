using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tayvey.Tools.Attributes;
using Tayvey.Tools.Interfaces;
using Tayvey.Tools.Models;

namespace Tayvey.Tools.Services
{
    /// <summary>
    /// MONGODB
    /// </summary>
    public class TvMongo : ITvMongo
    {
        /// <summary>
        /// 连接池
        /// </summary>
        private readonly List<TvMongoConnPool> _clients;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="configs"></param>
        public TvMongo(params TvMongoConnConfig[] configs)
        {
            // 检查是否存在重复KEY
            var config = configs.GroupBy(i => i.Key).FirstOrDefault(i => i.Count() > 1);
            if (config != null)
            {
                throw new Exception($"MONGODB初始化异常. 存在重复KEY. [{config.Key}]");
            }

            // 过滤无效KEY
            configs = configs.Where(i => !string.IsNullOrWhiteSpace(i.Key)).ToArray();

            // 创建连接
            _clients = configs.Select(config =>
            {
                // 连接设置
                var settings = MongoClientSettings.FromConnectionString(config.ConnStr);
                // 读取操作只返回已被大多数副本集成员确认的写操作的数据，确保数据在持久化到磁盘之前不会返回。这提供了更高的一致性，但可能导致较高的延迟。
                settings.ReadConcern = ReadConcern.Majority;
                // 写入操作需要大多数副本集成员确认，确保写操作已经持久化到大多数副本集成员的磁盘上。这提供了更高的可靠性，但可能导致较高的延迟。
                settings.WriteConcern = WriteConcern.WMajority;

                // 建立连接
                var client = new MongoClient(settings);

                return new TvMongoConnPool(config.Key, client);
            }).ToList();
        }

        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public MongoClient GetClient(string key)
        {
            var client = _clients.FirstOrDefault(i => i.Key == key)?.Client
                ?? throw new Exception($"MONGODB获取客户端异常. KEY不存在. [{key}]");

            return client;
        }

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public IMongoDatabase GetDatabase(string key, string dbName)
        {
            return GetClient(key).GetDatabase(dbName);
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dbName"></param>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public IMongoCollection<T> GetCollection<T>(string key, string dbName, string collectionName) where T : class, new()
        {
            return GetClient(key).GetDatabase(dbName).GetCollection<T>(collectionName);
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IMongoCollection<T> GetCollection<T>() where T : class, new()
        {
            var attr = typeof(T).GetCustomAttribute<TvMongoAttribute>()
                ?? throw new Exception($"MONGODB获取集合异常. 实体未添加特性TvMongoAttribute. [{typeof(T).FullName}]");

            return GetClient(attr._key).GetDatabase(attr._dbName).GetCollection<T>(attr._collectionName);
        }
    }
}