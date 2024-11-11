using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Tayvey.Tools.Helpers
{
    /// <summary>
    /// MONGODB 工具类
    /// </summary>
    public static class TvMongoHelper
    {
        /// <summary>
        /// 构建UPSERT
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static UpdateDefinition<T> TvMongoBuildUpsert<T>(this T entity, HashSet<string>? setOnInsertFields = null)
            where T : class, new()
        {
            setOnInsertFields ??= new HashSet<string>();

            var bson = entity.ToBsonDocument();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            UpdateDefinition<T>? upsert = null;
            foreach (var item in bson)
            {
                var name = item.Name;
                var value = item.Value;

                var prop = props.FirstOrDefault(x => x.Name == name || x.GetCustomAttribute<BsonElementAttribute>()?.ElementName == name);
                if (prop == null)
                {
                    continue;
                }

                var isUpsert = setOnInsertFields.Contains(name) || setOnInsertFields.Contains(prop.Name);

                upsert = isUpsert switch
                {
                    true when upsert == null => Builders<T>.Update.SetOnInsert(name, value),
                    true when upsert != null => upsert.SetOnInsert(name, value),
                    false when upsert == null => Builders<T>.Update.Set(name, value),
                    _ => upsert.Set(name, value)
                };
            }

            if (upsert == null)
            {
                throw new Exception("构建UPSERT异常");
            }

            return upsert;
        }
    }
}