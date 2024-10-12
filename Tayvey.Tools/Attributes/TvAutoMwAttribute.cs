using System;

namespace Tayvey.Tools.Attributes
{
    /// <summary>
    /// 自动注册中间件特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class TvAutoMwAttribute : Attribute
    {
        /// <summary>
        /// 排序号
        /// </summary>
        internal readonly uint _sort;

        /// <summary>
        /// 标识数组
        /// </summary>
        internal readonly string[] _marks;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="marks"></param>
        public TvAutoMwAttribute(uint sort = 0, params string[] marks)
        {
            _sort = sort;
            _marks = marks;
        }
    }
}