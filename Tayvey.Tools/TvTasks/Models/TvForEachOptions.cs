using System;

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
        public Action<TvForEachItem<T>>? TryAction { get; set; }

        /// <summary>
        /// 异常处理方法
        /// </summary>
        public Action<TvForEachItem<T>, Exception>? CatchAction { get; set; }

        /// <summary>
        /// 最大异步数量
        /// </summary>
        public uint? MaxConcurrency { get; set; }

        /// <summary>
        /// 是否分组异步遍历 (最大异步数量 > 1)
        /// </summary>
        public bool IsGroup { get; set; }
    }
}
