using System.Linq;
using MongoDB.Driver;
using StackExchange.Redis;
using System.Collections.Generic;
using System;

namespace Tayvey.Tools
{
    /// <summary>
    /// Redis仓储
    /// </summary>
    public class TvRedis : ITvRedis
    {
        /// <summary>
        /// 连接池
        /// </summary>
        private readonly List<TvRedisConnectionPool> _clients;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="configs"></param>
        public TvRedis(params TvRedisConnectionConfig[] configs)
        {
            // 创建连接
            _clients = configs.Select(config =>
            {
                var client = ConnectionMultiplexer.Connect(config.ConnectionStr);
                return new TvRedisConnectionPool(config, client);
            }).ToList();
        }

        /// <summary>
        /// 获取Redis客户端
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IConnectionMultiplexer GetClient(string key)
        {
            var db = _clients.FirstOrDefault(i => i.Config.Key == key)
                ?? throw new Exception($"Redis连接[{key}]不存在");

            return db.Client;
        }

        /// <summary>
        /// 获取Redis数据库
        /// </summary>
        /// <param name="key"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public IDatabase GetDatabase(string key, int db = -1)
        {
            var client = GetClient(key);
            return client.GetDatabase(db);
        }
    }
}