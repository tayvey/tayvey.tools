#if NETSTANDARD2_1
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tayvey.Tools.TvConfigs;
using Tayvey.Tools.TvMongos.Attrs;
using Tayvey.Tools.TvMongos.Models;
#endif

namespace Tayvey.Tools.TvMongos
{
    /// <summary>
    /// TvMongo
    /// </summary>
    public static class TvMongo
    {
        /// <summary>
        /// 连接字典
        /// </summary>
#if NET6_0_OR_GREATER
        private static readonly Lazy<Dictionary<string, MongoClient>> ClientDict = new(InitClientDict);
#else
        private static readonly Lazy<Dictionary<string, MongoClient>> ClientDict = new Lazy<Dictionary<string, MongoClient>>(InitClientDict);
#endif

        /// <summary>
        /// 初始化连接字典
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static Dictionary<string, MongoClient> InitClientDict()
        {
            // 没有读取到配置不做处理
            if (TvConfig.Options.TvMongo.Count == 0)
            {
#if NET8_0_OR_GREATER
                return [];
#elif NET6_0_OR_GREATER
                return new();
#else
                return new Dictionary<string, MongoClient>();
#endif
            }

            // 并行
            var mongoConfigPq = TvConfig.Options.TvMongo.AsParallel();

            // 检查有没有重复的key
            var allKeys = mongoConfigPq.Select(i => i.Key).ToList();
            var hsKeys = mongoConfigPq.Select(i => i.Key).ToHashSet();
            if (allKeys.Count != hsKeys.Count)
            {
                throw new Exception("配置Key重复");
            }

            // 检查有没有空Key
            if (hsKeys.Any(string.IsNullOrWhiteSpace))
            {
                throw new Exception("配置Key为空");
            }

            // 创建连接
            var clients = mongoConfigPq.Select(i =>
            {
                // 连接设置
                var settings = MongoClientSettings.FromConnectionString(i.ConnectionStr);
                // 读取操作只返回已被大多数副本集成员确认的写操作的数据，确保数据在持久化到磁盘之前不会返回。这提供了更高的一致性，但可能导致较高的延迟。
                settings.ReadConcern = ReadConcern.Majority;
                // 写入操作需要大多数副本集成员确认，确保写操作已经持久化到大多数副本集成员的磁盘上。这提供了更高的可靠性，但可能导致较高的延迟。
                settings.WriteConcern = WriteConcern.WMajority;

                // 建立连接
                var client = new MongoClient(settings);

                return new
                {
                    i.Key,
                    Client = client
                };
            });

            return clients.ToDictionary(i => i.Key, i => i.Client);
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbName">数据库名称</param>
        /// <param name="collectionName">集合名称</param>
        /// <param name="key">配置Key</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IMongoCollection<T> GetCollection<T>(string dbName, string collectionName = "", string key = "")
            where T : class, new()
        {
            // 无服务异常
            if (ClientDict.Value.Count == 0)
            {
                throw new Exception("未连接到任何MongoDB服务");
            }

            // 获取特性
            var attr = typeof(T).GetCustomAttribute<TvMongoAttribute>();

            // 集合名称
            collectionName = collectionName switch
            {
                // 传入的集合名称不为空则直接使用传入的集合名称
                _ when !string.IsNullOrWhiteSpace(collectionName) => collectionName,
                // 特性集合名称不为空则使用特性集合名称
                _ when !string.IsNullOrWhiteSpace(attr?.CollectionName) => attr.CollectionName,
                // 否则使用实体名称作为集合名称
                _ => typeof(T).Name
            };

            // 如果Key为空, 默认获取第一个连接
            if (string.IsNullOrWhiteSpace(key))
            {
                return ClientDict.Value.First().Value.GetDatabase(dbName).GetCollection<T>(collectionName);
            }

            return ClientDict.Value[key].GetDatabase(dbName).GetCollection<T>(collectionName);
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="dbName">数据库名称</param>
        /// <param name="collectionName">集合名称</param>
        /// <param name="key">配置Key</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IMongoCollection<BsonDocument> GetCollection(string dbName, string collectionName, string key = "")
        {
            // 无服务异常
            if (ClientDict.Value.Count == 0)
            {
                throw new Exception("未连接到任何MongoDB服务");
            }

            // 如果Key为空, 默认获取第一个连接
            if (string.IsNullOrWhiteSpace(key))
            {
                return ClientDict.Value.First().Value.GetDatabase(dbName).GetCollection<BsonDocument>(collectionName);
            }

            return ClientDict.Value[key].GetDatabase(dbName).GetCollection<BsonDocument>(collectionName);
        }
    }
}