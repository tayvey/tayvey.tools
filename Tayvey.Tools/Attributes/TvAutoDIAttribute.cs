using System;
using Tayvey.Tools.Enums;

namespace Tayvey.Tools.Attributes
{
    /// <summary>
    /// 自动依赖注入特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class TvAutoDIAttribute : Attribute
    {
        internal readonly TvAutoDILifeCycle _lifeCycle;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="lifeCycle"></param>
        public TvAutoDIAttribute(TvAutoDILifeCycle lifeCycle)
        {
            _lifeCycle = lifeCycle;
        }
    }
}