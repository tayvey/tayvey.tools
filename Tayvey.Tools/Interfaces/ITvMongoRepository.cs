using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;

namespace Tayvey.Tools.Interfaces
{
    /// <summary>
    /// MONGODB存储库接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITvMongoRepository<T> where T : class, new()
    {
        /// <summary>
        /// 获取集合操作对象
        /// </summary>
        /// <returns></returns>
        IMongoCollection<T> GetCollection();

        #region 查询
        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="asc"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        T Get(FilterDefinition<T> filter, Expression<Func<T, object>>? sort = null, bool asc = true, FindOptions? options = null);

        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="asc"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<T> GetAsync(FilterDefinition<T> filter, Expression<Func<T, object>>? sort = null, bool asc = true, FindOptions? options = null);

        /// <summary>
        /// 查询多条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="asc"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        List<T> GetList(FilterDefinition<T> filter, Expression<Func<T, object>>? sort = null, bool asc = true, FindOptions? options = null);

        /// <summary>
        /// 查询多条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="asc"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<List<T>> GetListAsync(FilterDefinition<T> filter, Expression<Func<T, object>>? sort = null, bool asc = true, FindOptions? options = null);

        /// <summary>
        /// 分页查询多条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sort"></param>
        /// <param name="asc"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        (List<T>, long) GetPageList(FilterDefinition<T> filter, int pageNumber, int pageSize, Expression<Func<T, object>>? sort = null, bool asc = true, FindOptions? options = null);

        /// <summary>
        /// 分页查询多条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sort"></param>
        /// <param name="asc"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<(List<T>, long)> GetPageListAsync(FilterDefinition<T> filter, int pageNumber, int pageSize, Expression<Func<T, object>>? sort = null, bool asc = true, FindOptions? options = null);

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        long Count(FilterDefinition<T> filter, FindOptions? options = null);

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<long> CountAsync(FilterDefinition<T> filter, FindOptions? options = null);

        /// <summary>
        /// 查询是否存在
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        bool Any(FilterDefinition<T> filter, FindOptions? options = null);

        /// <summary>
        /// 查询是否存在
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<bool> AnyAsync(FilterDefinition<T> filter, FindOptions? options = null);
        #endregion

        #region 新增
        /// <summary>
        /// 新增单条
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="options"></param>
        void Add(T entity, InsertOneOptions? options = null);

        /// <summary>
        /// 新增单条
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="options"></param>
        Task AddAsync(T entity, InsertOneOptions? options = null);

        /// <summary>
        /// 新增多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        void Add(IEnumerable<T> entities, InsertManyOptions? options = null);

        /// <summary>
        /// 新增多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        void Add(IEnumerable<InsertOneModel<T>> entities, BulkWriteOptions? options = null);

        /// <summary>
        /// 新增多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        Task AddAsync(IEnumerable<T> entities, InsertManyOptions? options = null);

        /// <summary>
        /// 新增多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        Task AddAsync(IEnumerable<InsertOneModel<T>> entities, BulkWriteOptions? options = null);
        #endregion

        #region 替换
        /// <summary>
        /// 替换单条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="entity"></param>
        /// <param name="options"></param>
        void Replace(FilterDefinition<T> filter, T entity, ReplaceOptions? options = null);

        /// <summary>
        /// 替换单条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="entity"></param>
        /// <param name="options"></param>
        Task ReplaceAsync(FilterDefinition<T> filter, T entity, ReplaceOptions? options = null);

        /// <summary>
        /// 替换多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        void Replace(IEnumerable<ReplaceOneModel<T>> entities, BulkWriteOptions? options = null);

        /// <summary>
        /// 替换多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        Task ReplaceAsync(IEnumerable<ReplaceOneModel<T>> entities, BulkWriteOptions? options = null);

        /// <summary>
        /// 获取并替换单条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="entity"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        T GetReplace(FilterDefinition<T> filter, T entity, FindOneAndReplaceOptions<T>? options = null);

        /// <summary>
        /// 获取并替换单条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="entity"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<T> GetReplaceAsync(FilterDefinition<T> filter, T entity, FindOneAndReplaceOptions<T>? options = null);
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="many"></param>
        /// <param name="options"></param>
        void Update(FilterDefinition<T> filter, UpdateDefinition<T> update, bool many = false, UpdateOptions? options = null);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="many"></param>
        /// <param name="options"></param>
        Task UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, bool many = false, UpdateOptions? options = null);

        /// <summary>
        /// 更新多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        void Update(IEnumerable<UpdateOneModel<T>> entities, BulkWriteOptions? options = null);

        /// <summary>
        /// 更新多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        Task UpdateAsync(IEnumerable<UpdateOneModel<T>> entities, BulkWriteOptions? options = null);

        /// <summary>
        /// 更新多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        void Update(IEnumerable<UpdateManyModel<T>> entities, BulkWriteOptions? options = null);

        /// <summary>
        /// 更新多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        Task UpdateAsync(IEnumerable<UpdateManyModel<T>> entities, BulkWriteOptions? options = null);

        /// <summary>
        /// 获取并更新单条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        T GetUpdate(FilterDefinition<T> filter, UpdateDefinition<T> update, FindOneAndUpdateOptions<T>? options = null);

        /// <summary>
        /// 获取并更新单条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<T> GetUpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, FindOneAndUpdateOptions<T>? options = null);
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="many"></param>
        void Del(FilterDefinition<T> filter, bool many = false);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="many"></param>
        Task DelAsync(FilterDefinition<T> filter, bool many = false);

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        void Del(IEnumerable<DeleteOneModel<T>> entities, BulkWriteOptions? options = null);

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        Task DelAsync(IEnumerable<DeleteOneModel<T>> entities, BulkWriteOptions? options = null);

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        void Del(IEnumerable<DeleteManyModel<T>> entities, BulkWriteOptions? options = null);

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        Task DelAsync(IEnumerable<DeleteManyModel<T>> entities, BulkWriteOptions? options = null);

        /// <summary>
        /// 获取并删除单条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        T GetDel(FilterDefinition<T> filter, FindOneAndDeleteOptions<T>? options = null);

        /// <summary>
        /// 获取并删除单条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<T> GetDelAsync(FilterDefinition<T> filter, FindOneAndDeleteOptions<T>? options = null);
        #endregion
    }
}