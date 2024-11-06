using MongoDB.Driver;
using SqlSugar;
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
    /// SQLSUGAR
    /// </summary>
    public class TvSqlSugar : ITvSqlSugar
    {
        /// <summary>
        /// 连接池
        /// </summary>
        private readonly List<TvSqlSugarConnPool> _clients;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="configs"></param>
        public TvSqlSugar(params TvSqlSugarConnConfig[] configs)
        {
            // 检查是否存在重复KEY
            var config = configs.GroupBy(i => i.Key).FirstOrDefault(i => i.Count() > 1);
            if (config != null)
            {
                throw new Exception($"SQLSUGAR初始化异常. 存在重复KEY. [{config.Key}]");
            }

            // 过滤无效KEY
            configs = configs.Where(i => !string.IsNullOrWhiteSpace(i.Key)).ToArray();

            // 创建连接
            _clients = configs.Select(config =>
            {
                var client = new SqlSugarScope(new ConnectionConfig
                {
                    DbType = config.Type,
                    ConnectionString = config.ConnStr,
                    IsAutoCloseConnection = true,
                    ConfigId = config.Key
                });

                return new TvSqlSugarConnPool(config.Key, client);
            }).ToList();
        }

        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <returns></returns>
        public SqlSugarScope GetClient<T>()
             where T : class, new()
        {
            var attr = typeof(T).GetCustomAttribute<TvSqlSugarAttribute>()
                ?? throw new Exception($"SQLSUGAR获取集合异常. 实体未添加特性TvSqlSugarAttribute. [{typeof(T).FullName}]");

            var client = _clients.FirstOrDefault(i => i.Key == attr._key)?.Client
                ?? throw new Exception($"SQLSUGAR获取客户端异常. KEY不存在. [{attr._key}]");

            return client;
        }
    }
}