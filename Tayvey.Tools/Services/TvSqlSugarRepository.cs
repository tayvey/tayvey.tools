using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tayvey.Tools.Interfaces;

namespace Tayvey.Tools.Services
{
    /// <summary>
    /// SQLSUGAR存储库
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TvSqlSugarRepository<T> : ITvSqlSugarRepository<T>
        where T : class, new()
    {
        /// <summary>
        /// 操作对象
        /// </summary>
        private readonly SqlSugarScope _client;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="tvSqlSugar"></param>
        public TvSqlSugarRepository(ITvSqlSugar tvSqlSugar)
        {
            _client = tvSqlSugar.GetClient<T>();
        }

        /// <summary>
        /// 获取操作对象
        /// </summary>
        /// <returns></returns>
        public SqlSugarScope GetClient() => _client;

        #region 查询
        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public T Get(Expression<Func<T, bool>> filter, Expression<Func<T, object>>? sort = null, bool asc = true)
        {
            return _client.Queryable<T>()
                .OrderByIF(sort != null, sort, asc ? OrderByType.Asc : OrderByType.Desc)
                .First(filter);
        }

        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public Task<T> GetAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>>? sort = null, bool asc = true)
        {
            return _client.Queryable<T>()
                .OrderByIF(sort != null, sort, asc ? OrderByType.Asc : OrderByType.Desc)
                .FirstAsync(filter);
        }

        /// <summary>
        /// 查询多条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public List<T> GetList(Expression<Func<T, bool>> filter, Expression<Func<T, object>>? sort = null, bool asc = true)
        {
            return _client.Queryable<T>()
                .Where(filter)
                .OrderByIF(sort != null, sort, asc ? OrderByType.Asc : OrderByType.Desc)
                .ToList();
        }

        /// <summary>
        /// 查询多条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public Task<List<T>> GetListAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>>? sort = null, bool asc = true)
        {
            return _client.Queryable<T>()
                .Where(filter)
                .OrderByIF(sort != null, sort, asc ? OrderByType.Asc : OrderByType.Desc)
                .ToListAsync();
        }

        /// <summary>
        /// 分页查询多条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sort"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public (List<T>, int) GetPageList(Expression<Func<T, bool>> filter, int pageNumber, int pageSize, Expression<Func<T, object>>? sort = null, bool asc = true)
        {
            var count = 0;

            var data = _client.Queryable<T>()
                .Where(filter)
                .OrderByIF(sort != null, sort, asc ? OrderByType.Asc : OrderByType.Desc)
                .ToPageList(pageNumber, pageSize, ref count);

            return (data, count);
        }

        /// <summary>
        /// 分页查询多条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sort"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public async Task<(List<T>, int)> GetPageListAsync(Expression<Func<T, bool>> filter, int pageNumber, int pageSize, Expression<Func<T, object>>? sort = null, bool asc = true)
        {
            RefAsync<int> count = 0;

            var data = await _client.Queryable<T>()
                .Where(filter)
                .OrderByIF(sort != null, sort, asc ? OrderByType.Asc : OrderByType.Desc)
                .ToPageListAsync(pageNumber, pageSize, count);

            return (data, count.Value);
        }

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> filter)
        {
            return _client.Queryable<T>().Count(filter);
        }

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Task<int> CountAsync(Expression<Func<T, bool>> filter)
        {
            return _client.Queryable<T>().CountAsync(filter);
        }

        /// <summary>
        /// 查询是否存在
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public bool Any(Expression<Func<T, bool>> filter)
        {
            return _client.Queryable<T>().Any(filter);
        }

        /// <summary>
        /// 查询是否存在
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            return _client.Queryable<T>().AnyAsync(filter);
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增单条
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            _client.Insertable(entity).ExecuteCommand();
        }

        /// <summary>
        /// 新增单条
        /// </summary>
        /// <param name="entity"></param>
        public Task AddAsync(T entity)
        {
            return _client.Insertable(entity).ExecuteCommandAsync();
        }

        /// <summary>
        /// 新增多条
        /// </summary>
        /// <param name="entities"></param>
        public void Add(IEnumerable<T> entities)
        {
            _client.Insertable<T>(entities).ExecuteCommand();
        }

        /// <summary>
        /// 新增多条
        /// </summary>
        /// <param name="entities"></param>
        public Task AddAsync(IEnumerable<T> entities)
        {
            return _client.Insertable<T>(entities).ExecuteCommandAsync();
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            _client.Updateable(entity).ExecuteCommand();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        public Task UpdateAsync(T entity)
        {
            return _client.Updateable(entity).ExecuteCommandAsync();
        }

        /// <summary>
        /// 更新多条
        /// </summary>
        /// <param name="entities"></param>
        public void Update(IEnumerable<T> entities)
        {
            _client.Updateable<T>(entities).ExecuteCommand();
        }

        /// <summary>
        /// 更新多条
        /// </summary>
        /// <param name="entities"></param>
        public Task UpdateAsync(IEnumerable<T> entities)
        {
            return _client.Updateable<T>(entities).ExecuteCommandAsync();
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        public void Del(T entity)
        {
            _client.Deleteable(entity).ExecuteCommand();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        public Task DelAsync(T entity)
        {
            return _client.Deleteable(entity).ExecuteCommandAsync();
        }

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="entities"></param>
        public void Del(IEnumerable<T> entities)
        {
            _client.Deleteable<T>(entities).ExecuteCommand();
        }

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="entities"></param>
        public Task DelAsync(IEnumerable<T> entities)
        {
            return _client.Deleteable<T>(entities).ExecuteCommandAsync();
        }
        #endregion
    }
}