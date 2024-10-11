//#if NETSTANDARD2_1
//using StackExchange.Redis;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Tayvey.Tools.TvConfigs;
//#endif

//namespace Tayvey.Tools.TvRedises
//{
//    /// <summary>
//    /// TvRedis
//    /// </summary>
//    public static class TvRedis
//    {
//        /// <summary>
//        /// 连接字典
//        /// </summary>
//#if NET6_0_OR_GREATER
//        private static readonly Lazy<Dictionary<string, ConnectionMultiplexer>> ClientDict = new(InitClientDict);
//#else
//        private static readonly Lazy<Dictionary<string, ConnectionMultiplexer>> ClientDict = new Lazy<Dictionary<string, ConnectionMultiplexer>>(InitClientDict);
//#endif

//        /// <summary>
//        /// 初始化连接字典
//        /// </summary>
//        /// <returns></returns>
//        /// <exception cref="Exception"></exception>
//        private static Dictionary<string, ConnectionMultiplexer> InitClientDict()
//        {
//            // 没有读取到配置不做处理
//            if (TvConfig.Options.TvRedis.Count == 0)
//            {
//#if NET8_0_OR_GREATER
//                return [];
//#elif NET6_0_OR_GREATER
//                return new();
//#else
//                return new Dictionary<string, ConnectionMultiplexer>();
//#endif
//            }

//            // 并行
//            var redisConfigPq = TvConfig.Options.TvRedis.AsParallel();

//            // 检查有没有重复的key
//            var allKeys = redisConfigPq.Select(i => i.Key).ToList();
//            var hsKeys = redisConfigPq.Select(i => i.Key).ToHashSet();
//            if (allKeys.Count != hsKeys.Count)
//            {
//                throw new Exception("配置Key重复");
//            }

//            // 检查有没有空KEY
//            if (hsKeys.Any(string.IsNullOrWhiteSpace))
//            {
//                throw new Exception("配置Key为空");
//            }

//            // 创建连接
//            var clients = redisConfigPq.Select(i => new
//            {
//                i.Key,
//                Client = ConnectionMultiplexer.Connect(i.ConnectionStr)
//            });

//            return clients.ToDictionary(i => i.Key, i => i.Client);
//        }

//        /// <summary>
//        /// 获取客户端
//        /// </summary>
//        /// <param name="key">配置Key</param>
//        /// <returns></returns>
//        public static ConnectionMultiplexer GetClient(string key = "")
//        {
//            // 无服务异常
//            if (ClientDict.Value.Count == 0)
//            {
//                throw new Exception("未连接到任何Redis服务");
//            }

//            // 如果Key为空, 默认获取第一个连接
//            if (string.IsNullOrWhiteSpace(key))
//            {
//                return ClientDict.Value.First().Value;
//            }

//            return ClientDict.Value[key];
//        }

//        /// <summary>
//        /// 获取redis数据库
//        /// </summary>
//        /// <param name="key">配置Key</param>
//        /// <param name="db">库编号</param>
//        /// <returns></returns>
//        public static IDatabase GetDatabase(string key = "", int db = -1)
//        {
//            // 无服务异常
//            if (ClientDict.Value.Count == 0)
//            {
//                throw new Exception("未连接到任何Redis服务");
//            }

//            // 如果Key为空, 默认获取第一个连接
//            if (string.IsNullOrWhiteSpace(key))
//            {
//                return ClientDict.Value.First().Value.GetDatabase(db);
//            }

//            return ClientDict.Value[key].GetDatabase(db);
//        }
//    }
//}