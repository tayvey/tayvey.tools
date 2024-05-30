#if NETSTANDARD2_1
using System;
#endif

namespace Tayvey.Tools.TvMongos.Attrs
{
    /// <summary>
    /// TvMongo实体集合特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class TvMongoAttribute : Attribute
    {
        /// <summary>
        /// 集合名称
        /// </summary>
        public string CollectionName { get; } = "";

        /// <summary>
        /// 初始化构造
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        public TvMongoAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }

        /// <summary>
        /// 私有空构造
        /// </summary>
        private TvMongoAttribute() { }
    }
}