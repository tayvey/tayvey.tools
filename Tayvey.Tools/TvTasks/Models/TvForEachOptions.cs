#if NET6_0_OR_GREATER
#elif NETSTANDARD2_1
using System;
#endif

namespace Tayvey.Tools.TvTasks.Models
{
    /// <summary>
    /// Tv异步遍历选项
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public sealed class TvForEachOptions<T>
    {
        /// <summary>
        /// 数据处理方法
        /// </summary>
#if NET6_0_OR_GREATER
        public Action<TvForEachItem<T>>? TryAction { get; init; }
#elif NETSTANDARD2_1
        public Action<TvForEachItem<T>>? TryAction { get; }
#endif

        /// <summary>
        /// 异常处理方法
        /// </summary>
#if NET6_0_OR_GREATER
        public Action<TvForEachItem<T>, Exception>? CatchAction { get; init; }
#elif NETSTANDARD2_1
        public Action<TvForEachItem<T>, Exception>? CatchAction { get; }
#endif

        /// <summary>
        /// 最大异步数量
        /// </summary>
#if NET6_0_OR_GREATER
        public uint? MaxConcurrency { get; init; }
#elif NETSTANDARD2_1
        public uint? MaxConcurrency { get; }
#endif

        /// <summary>
        /// 是否分组异步遍历 (最大异步数量 > 1)
        /// </summary>
#if NET6_0_OR_GREATER
        public bool IsGroup { get; init; }
#elif NETSTANDARD2_1
        public bool IsGroup { get; }
#endif

#if NET6_0_OR_GREATER
#elif NETSTANDARD2_1
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="tryAction">数据处理方法</param>
        /// <param name="catchAction">异常处理方法</param>
        /// <param name="maxConcurrency">最大异步数量</param>
        /// <param name="isGroup">是否分组异步遍历 (最大异步数量 > 1)</param>
        public TvForEachOptions(
            Action<TvForEachItem<T>>? tryAction,
            Action<TvForEachItem<T>, Exception>? catchAction,
            uint? maxConcurrency = null,
            bool isGroup = false
        )
        {
            TryAction = tryAction;
            CatchAction = catchAction;
            MaxConcurrency = maxConcurrency;
            IsGroup = isGroup;
        }
#endif
    }
}
