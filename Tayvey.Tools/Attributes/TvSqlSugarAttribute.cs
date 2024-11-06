using System;

namespace Tayvey.Tools.Attributes
{
    /// <summary>
    /// SQLSUGAR特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class TvSqlSugarAttribute : Attribute
    {
        /// <summary>
        /// KEY
        /// </summary>
        internal readonly string _key;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="key"></param>
        public TvSqlSugarAttribute(string key)
        {
            _key = key;
        }
    }
}