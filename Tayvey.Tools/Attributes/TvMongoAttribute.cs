using System;

namespace Tayvey.Tools.Attributes
{
    /// <summary>
    /// MONGODB特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class TvMongoAttribute : Attribute
    {
        /// <summary>
        /// KEY
        /// </summary>
        internal readonly string _key;

        /// <summary>
        /// 数据库
        /// </summary>
        internal readonly string _dbName;

        /// <summary>
        /// 集合
        /// </summary>
        internal readonly string _collectionName;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbName"></param>
        /// <param name="collectionName"></param>
        public TvMongoAttribute(string key, string dbName, string collectionName)
        {
            _key = key;
            _dbName = dbName;
            _collectionName = collectionName;
        }
    }
}