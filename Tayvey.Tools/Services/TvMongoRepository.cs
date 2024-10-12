using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tayvey.Tools.Interfaces;

namespace Tayvey.Tools.Services
{
    /// <summary>
    /// MONGODB存储库
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TvMongoRepository<T> : ITvMongoRepository<T>
        where T : class, new()
    {
        /// <summary>
        /// 集合操作对象
        /// </summary>
        private readonly IMongoCollection<T> _collection;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="tvMongo"></param>
        public TvMongoRepository(ITvMongo tvMongo)
        {
            _collection = tvMongo.GetCollection<T>();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="tvMongo"></param>
        /// <param name="key"></param>
        /// <param name="dbName"></param>
        /// <param name="collectionName"></param>
        public TvMongoRepository(ITvMongo tvMongo, string key, string dbName, string collectionName)
        {
            _collection = tvMongo.GetCollection<T>(key, dbName, collectionName);
        }

        /// <summary>
        /// 获取集合操作对象
        /// </summary>
        /// <returns></returns>
        public IMongoCollection<T> GetCollection() => _collection;

        #region 查询
        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="asc"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public T Get(FilterDefinition<T> filter, Expression<Func<T, object>>? sort = null, bool asc = true, FindOptions? options = null)
        {
            var find = _collection.Find(filter, options);

            if (sort != null && asc)
            {
                return find.SortBy(sort).FirstOrDefault();
            }

            if (sort != null && !asc)
            {
                return find.SortByDescending(sort).FirstOrDefault();
            }

            return find.FirstOrDefault();
        }

        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="asc"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public Task<T> GetAsync(FilterDefinition<T> filter, Expression<Func<T, object>>? sort = null, bool asc = true, FindOptions? options = null)
        {
            var find = _collection.Find(filter, options);

            if (sort != null && asc)
            {
                return find.SortBy(sort).FirstOrDefaultAsync();
            }

            if (sort != null && !asc)
            {
                return find.SortByDescending(sort).FirstOrDefaultAsync();
            }

            return find.FirstOrDefaultAsync();
        }

        /// <summary>
        /// 查询多条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="asc"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public List<T> GetList(FilterDefinition<T> filter, Expression<Func<T, object>>? sort = null, bool asc = true, FindOptions? options = null)
        {
            var find = _collection.Find(filter, options);

            if (sort != null && asc)
            {
                return find.SortBy(sort).ToList();
            }

            if (sort != null && !asc)
            {
                return find.SortByDescending(sort).ToList();
            }

            return find.ToList();
        }

        /// <summary>
        /// 查询多条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="asc"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public Task<List<T>> GetListAsync(FilterDefinition<T> filter, Expression<Func<T, object>>? sort = null, bool asc = true, FindOptions? options = null)
        {
            var find = _collection.Find(filter, options);

            if (sort != null && asc)
            {
                return find.SortBy(sort).ToListAsync();
            }

            if (sort != null && !asc)
            {
                return find.SortByDescending(sort).ToListAsync();
            }

            return find.ToListAsync();
        }

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
        public (List<T>, long) GetPageList(FilterDefinition<T> filter, int pageNumber, int pageSize, Expression<Func<T, object>>? sort = null, bool asc = true, FindOptions? options = null)
        {
            var find = _collection.Find(filter, options);
            var count = find.CountDocuments();

            if (sort != null && asc)
            {
                var data = find.SortBy(sort).Skip((pageNumber - 1) * pageSize).Limit(pageSize).ToList();
                return (data, count);
            }

            if (sort != null && !asc)
            {
                var data = find.SortByDescending(sort).Skip((pageNumber - 1) * pageSize).Limit(pageSize).ToList();
                return (data, count);
            }

            {
                var data = find.Skip((pageNumber - 1) * pageSize).Limit(pageSize).ToList();
                return (data, count);
            }
        }

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
        public async Task<(List<T>, long)> GetPageListAsync(FilterDefinition<T> filter, int pageNumber, int pageSize, Expression<Func<T, object>>? sort = null, bool asc = true, FindOptions? options = null)
        {
            var find = _collection.Find(filter, options);
            var count = find.CountDocuments();

            if (sort != null && asc)
            {
                var data = await find.SortBy(sort).Skip((pageNumber - 1) * pageSize).Limit(pageSize).ToListAsync();
                return (data, count);
            }

            if (sort != null && !asc)
            {
                var data = await find.SortByDescending(sort).Skip((pageNumber - 1) * pageSize).Limit(pageSize).ToListAsync();
                return (data, count);
            }

            {
                var data = await find.Skip((pageNumber - 1) * pageSize).Limit(pageSize).ToListAsync();
                return (data, count);
            }
        }

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public long Count(FilterDefinition<T> filter, FindOptions? options = null)
        {
            return _collection.Find(filter, options).CountDocuments();
        }

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public Task<long> CountAsync(FilterDefinition<T> filter, FindOptions? options = null)
        {
            return _collection.Find(filter, options).CountDocumentsAsync();
        }

        /// <summary>
        /// 查询是否存在
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public bool Any(FilterDefinition<T> filter, FindOptions? options = null)
        {
            return _collection.Find(filter, options).Any();
        }

        /// <summary>
        /// 查询是否存在
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public Task<bool> AnyAsync(FilterDefinition<T> filter, FindOptions? options = null)
        {
            return _collection.Find(filter, options).AnyAsync();
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增单条
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="options"></param>
        public void Add(T entity, InsertOneOptions? options = null)
        {
            _collection.InsertOne(entity, options);
        }

        /// <summary>
        /// 新增单条
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="options"></param>
        public Task AddAsync(T entity, InsertOneOptions? options = null)
        {
            return _collection.InsertOneAsync(entity, options);
        }

        /// <summary>
        /// 新增多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        public void Add(IEnumerable<T> entities, InsertManyOptions? options = null)
        {
            _collection.InsertMany(entities, options);
        }

        /// <summary>
        /// 新增多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        public void Add(IEnumerable<InsertOneModel<T>> entities, BulkWriteOptions? options = null)
        {
            _collection.BulkWrite(entities, options);
        }

        /// <summary>
        /// 新增多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        public Task AddAsync(IEnumerable<T> entities, InsertManyOptions? options = null)
        {
            return _collection.InsertManyAsync(entities, options);
        }

        /// <summary>
        /// 新增多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        public Task AddAsync(IEnumerable<InsertOneModel<T>> entities, BulkWriteOptions? options = null)
        {
            return _collection.BulkWriteAsync(entities, options);
        }
        #endregion

        #region 替换
        /// <summary>
        /// 替换单条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="entity"></param>
        /// <param name="options"></param>
        public void Replace(FilterDefinition<T> filter, T entity, ReplaceOptions? options = null)
        {
            _collection.ReplaceOne(filter, entity, options);
        }

        /// <summary>
        /// 替换单条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="entity"></param>
        /// <param name="options"></param>
        public Task ReplaceAsync(FilterDefinition<T> filter, T entity, ReplaceOptions? options = null)
        {
            return _collection.ReplaceOneAsync(filter, entity, options);
        }

        /// <summary>
        /// 替换多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        public void Replace(IEnumerable<ReplaceOneModel<T>> entities, BulkWriteOptions? options = null)
        {
            _collection.BulkWrite(entities, options);
        }

        /// <summary>
        /// 替换多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        public Task ReplaceAsync(IEnumerable<ReplaceOneModel<T>> entities, BulkWriteOptions? options = null)
        {
            return _collection.BulkWriteAsync(entities, options);
        }

        /// <summary>
        /// 获取并替换单条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="entity"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public T GetReplace(FilterDefinition<T> filter, T entity, FindOneAndReplaceOptions<T>? options = null)
        {
            return _collection.FindOneAndReplace(filter, entity, options);
        }

        /// <summary>
        /// 获取并替换单条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="entity"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public Task<T> GetReplaceAsync(FilterDefinition<T> filter, T entity, FindOneAndReplaceOptions<T>? options = null)
        {
            return _collection.FindOneAndReplaceAsync(filter, entity, options);
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="many"></param>
        /// <param name="options"></param>
        public void Update(FilterDefinition<T> filter, UpdateDefinition<T> update, bool many = false, UpdateOptions? options = null)
        {
            if (many)
            {
                _collection.UpdateMany(filter, update, options);
            }
            else
            {
                _collection.UpdateOne(filter, update, options);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="many"></param>
        /// <param name="options"></param>
        public Task UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, bool many = false, UpdateOptions? options = null)
        {
            if (many)
            {
                return _collection.UpdateManyAsync(filter, update, options);
            }
            else
            {
                return _collection.UpdateOneAsync(filter, update, options);
            }
        }

        /// <summary>
        /// 更新多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        public void Update(IEnumerable<UpdateOneModel<T>> entities, BulkWriteOptions? options = null)
        {
            _collection.BulkWrite(entities, options);
        }

        /// <summary>
        /// 更新多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        public Task UpdateAsync(IEnumerable<UpdateOneModel<T>> entities, BulkWriteOptions? options = null)
        {
            return _collection.BulkWriteAsync(entities, options);
        }

        /// <summary>
        /// 更新多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        public void Update(IEnumerable<UpdateManyModel<T>> entities, BulkWriteOptions? options = null)
        {
            _collection.BulkWrite(entities, options);
        }

        /// <summary>
        /// 更新多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        public Task UpdateAsync(IEnumerable<UpdateManyModel<T>> entities, BulkWriteOptions? options = null)
        {
            return _collection.BulkWriteAsync(entities, options);
        }

        /// <summary>
        /// 获取并更新单条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public T GetUpdate(FilterDefinition<T> filter, UpdateDefinition<T> update, FindOneAndUpdateOptions<T>? options = null)
        {
            return _collection.FindOneAndUpdate(filter, update, options);
        }

        /// <summary>
        /// 获取并更新单条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public Task<T> GetUpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, FindOneAndUpdateOptions<T>? options = null)
        {
            return _collection.FindOneAndUpdateAsync(filter, update, options);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="many"></param>
        public void Del(FilterDefinition<T> filter, bool many = false)
        {
            if (many)
            {
                _collection.DeleteMany(filter);
            }
            else
            {
                _collection.DeleteOne(filter);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="many"></param>
        public Task DelAsync(FilterDefinition<T> filter, bool many = false)
        {
            if (many)
            {
                return _collection.DeleteManyAsync(filter);
            }
            else
            {
                return _collection.DeleteOneAsync(filter);
            }
        }

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        public void Del(IEnumerable<DeleteOneModel<T>> entities, BulkWriteOptions? options = null)
        {
            _collection.BulkWrite(entities, options);
        }

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        public Task DelAsync(IEnumerable<DeleteOneModel<T>> entities, BulkWriteOptions? options = null)
        {
            return _collection.BulkWriteAsync(entities, options);
        }

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        public void Del(IEnumerable<DeleteManyModel<T>> entities, BulkWriteOptions? options = null)
        {
            _collection.BulkWrite(entities, options);
        }

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        public Task DelAsync(IEnumerable<DeleteManyModel<T>> entities, BulkWriteOptions? options = null)
        {
            return _collection.BulkWriteAsync(entities, options);
        }

        /// <summary>
        /// 获取并删除单条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public T GetDel(FilterDefinition<T> filter, FindOneAndDeleteOptions<T>? options = null)
        {
            return _collection.FindOneAndDelete(filter, options);
        }

        /// <summary>
        /// 获取并删除单条
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public Task<T> GetDelAsync(FilterDefinition<T> filter, FindOneAndDeleteOptions<T>? options = null)
        {
            return _collection.FindOneAndDeleteAsync(filter, options);
        }
        #endregion
    }
}