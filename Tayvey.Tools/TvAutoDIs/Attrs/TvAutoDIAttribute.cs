#if NET6_0_OR_GREATER
#elif NETSTANDARD2_1
using System;
using Tayvey.Tools.TvAutoDIs.Enums;
#endif

namespace Tayvey.Tools.TvAutoDIs.Attrs
{
    /// <summary>
    /// Tv自动依赖注入特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class TvAutoDIAttribute : Attribute
    {
        /// <summary>
        /// 生命周期
        /// </summary>
        public TvAutoDILifeCycle LifeCycle { get; } = TvAutoDILifeCycle.Scoped;

        /// <summary>
        /// 是否忽略接口
        /// </summary>
        public bool IgnoreInterface { get; } = false;

        /// <summary>
        /// 初始化
        /// </summary>
        public TvAutoDIAttribute()
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="lifeCycle">生命周期</param>
        /// <param name="ignoreInterface">是否忽略接口</param>
        public TvAutoDIAttribute(TvAutoDILifeCycle lifeCycle, bool ignoreInterface = false)
        {
            LifeCycle = lifeCycle;
            IgnoreInterface = ignoreInterface;
        }
    }
}
