using System.Linq;
using System;
using Tayvey.Tools.Models;
using MongoDB.Driver;
using StackExchange.Redis;
using System.Collections.Generic;
using Tayvey.Tools.Interfaces;

namespace Tayvey.Tools.Services
{
    /// <summary>
    /// REDIS
    /// </summary>
    public class TvRedis : ITvRedis
    {
        /// <summary>
        /// 连接池
        /// </summary>
        private readonly List<TvRedisConnPool> _clients;
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="configs"></param>
        /// <exception cref="Exception"></exception>
        public TvRedis(params TvRedisConnConfig[] configs)
        {
            // 检查是否存在重复KEY
            var config = configs.GroupBy(i => i.Key).FirstOrDefault(i => i.Count() > 1);
            if (config != null)
            {
                throw new Exception($"REDIS初始化异常. 存在重复KEY. [{config.Key}]");
            }

            // 过滤无效KEY
            configs = configs.Where(i => !string.IsNullOrWhiteSpace(i.Key)).ToArray();

            // 创建连接
            _clients = configs.Select(config =>
            {
                var client = ConnectionMultiplexer.Connect(config.ConnStr);
                return new TvRedisConnPool(config.Key, client);
            }).ToList();
        }

        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ConnectionMultiplexer GetClient(string key)
        {
            var client = _clients.FirstOrDefault(i => i.Key == key)?.Client
                ?? throw new Exception($"REDIS获取客户端异常. KEY不存在. [{key}]");

            return client;
        }

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="key"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public IDatabase GetDatabase(string key, int db = -1)
        {
            return GetClient(key).GetDatabase(db);
        }
    }
}