using MongoDB.Driver;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tayvey.Tools
{
    /// <summary>
    /// 关系型数据库仓储
    /// </summary>
    public class TvSql : ITvSql
    {
        /// <summary>
        /// 连接池
        /// </summary>
        private readonly List<TvSqlConnectionPool> _clients;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="configs"></param>
        public TvSql(params TvSqlConnectionConfig[] configs)
        {
            // 创建连接
            _clients = configs.Select(config =>
            {
                var client = new SqlSugarScope(new ConnectionConfig
                {
                    DbType = config.Type,
                    ConnectionString = config.ConnectionStr,
                    IsAutoCloseConnection = true,
                    ConfigId = config.Key
                });

                return new TvSqlConnectionPool(config, client);
            }).ToList();
        }

        /// <summary>
        /// 获取数据库客户端
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ISqlSugarClient GetClient(string key)
        {
            var db = _clients.FirstOrDefault(i => i.Config.Key == key)
                ?? throw new Exception($"数据库连接[{key}]不存在");

            return db.Client;
        }
    }
}