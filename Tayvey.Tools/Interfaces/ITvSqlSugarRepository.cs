using SqlSugar;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;

namespace Tayvey.Tools.Interfaces
{
    /// <summary>
    /// SQLSUGAR存储库接口
    /// </summary>
    public interface ITvSqlSugarRepository<T> where T : class, new()
    {
        /// <summary>
        /// 获取操作对象
        /// </summary>
        /// <returns></returns>
        public SqlSugarScope GetClient();

        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public T Get(Expression<Func<T, bool>> filter, Expression<Func<T, object>>? sort = null, bool asc = true);

        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public Task<T> GetAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>>? sort = null, bool asc = true);

        /// <summary>
        /// 查询多条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public List<T> GetList(Expression<Func<T, bool>> filter, Expression<Func<T, object>>? sort = null, bool asc = true);

        /// <summary>
        /// 查询多条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public Task<List<T>> GetListAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>>? sort = null, bool asc = true);

        /// <summary>
        /// 分页查询多条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sort"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public (List<T>, int) GetPageList(Expression<Func<T, bool>> filter, int pageNumber, int pageSize, Expression<Func<T, object>>? sort = null, bool asc = true);

        /// <summary>
        /// 分页查询多条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sort"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public Task<(List<T>, int)> GetPageListAsync(Expression<Func<T, bool>> filter, int pageNumber, int pageSize, Expression<Func<T, object>>? sort = null, bool asc = true);

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> filter);

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Task<int> CountAsync(Expression<Func<T, bool>> filter);

        /// <summary>
        /// 查询是否存在
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public bool Any(Expression<Func<T, bool>> filter);

        /// <summary>
        /// 查询是否存在
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Task<bool> AnyAsync(Expression<Func<T, bool>> filter);

        /// <summary>
        /// 新增单条
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity);

        /// <summary>
        /// 新增单条
        /// </summary>
        /// <param name="entity"></param>
        public Task AddAsync(T entity);

        /// <summary>
        /// 新增多条
        /// </summary>
        /// <param name="entities"></param>
        public void Add(IEnumerable<T> entities);

        /// <summary>
        /// 新增多条
        /// </summary>
        /// <param name="entities"></param>
        public Task AddAsync(IEnumerable<T> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        public Task UpdateAsync(T entity);

        /// <summary>
        /// 更新多条
        /// </summary>
        /// <param name="entities"></param>
        public void Update(IEnumerable<T> entities);

        /// <summary>
        /// 更新多条
        /// </summary>
        /// <param name="entities"></param>
        public Task UpdateAsync(IEnumerable<T> entities);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        public void Del(T entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        public Task DelAsync(T entity);

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="entities"></param>
        public void Del(IEnumerable<T> entities);

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="entities"></param>
        public Task DelAsync(IEnumerable<T> entities);
    }
}