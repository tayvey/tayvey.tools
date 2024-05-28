#if NETSTANDARD2_1
using System;
using System.Collections.Generic;
using Tayvey.Tools.TvAutoDIs.Attrs;
#endif

namespace Tayvey.Tools.TvAutoDIs.Models
{
    /// <summary>
    /// Tv自动依赖注入的类实体
    /// </summary>
    internal sealed class TvAutoDIClass
    {
        /// <summary>
        /// 类
        /// </summary>
        internal Type Clz { get; }

        /// <summary>
        /// 直接实现的接口列表
        /// </summary>
#if NET8_0_OR_GREATER
        internal List<Type> Interfaces = [];
#elif NET6_0_OR_GREATER
        internal List<Type> Interfaces = new();
#else
        internal List<Type> Interfaces { get; set; } = new List<Type>();
#endif

        /// <summary>
        /// 特性
        /// </summary>
        internal TvAutoDIAttribute Attr { get; }

        /// <summary>
        /// 初始化构造
        /// </summary>
        /// <param name="clz">类</param>
        /// <param name="attr">特性</param>
        internal TvAutoDIClass(Type clz, TvAutoDIAttribute attr)
        {
            Clz = clz;
            Attr = attr;
        }
    }
}