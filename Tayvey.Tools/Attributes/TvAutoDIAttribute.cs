using System;

namespace Tayvey.Tools
{
    /// <summary>
    /// 自动依赖注入特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class TvAutoDIAttribute : Attribute
    {
        /// <summary>
        /// 生命周期
        /// </summary>
        internal readonly TvAutoDILifeCycle _lifeCycle;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="lifeCycle"></param>
        public TvAutoDIAttribute(TvAutoDILifeCycle lifeCycle = TvAutoDILifeCycle.Scoped)
        {
            _lifeCycle = lifeCycle;
        }
    }
}