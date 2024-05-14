namespace Tayvey.Tools.TvTasks.Models
{
    /// <summary>
    /// Tv异步遍历项
    /// </summary>
    /// <typeparam name="T"></typeparam>
#if NET8_0_OR_GREATER
    /// <param name="item">数据</param>
    /// <param name="index">索引</param>
    public sealed class TvForEachItem<T>(T item, uint index)
#elif NET6_0 || NETSTANDARD2_1
    public sealed class TvForEachItem<T>
#endif
    {
        /// <summary>
        /// 数据
        /// </summary>
#if NET8_0_OR_GREATER
        public T Item { get; } = item;
#elif NET6_0 || NETSTANDARD2_1
        public T Item { get; }
#endif

        /// <summary>
        /// 索引
        /// </summary>
#if NET8_0_OR_GREATER
        public uint Index { get; } = index;
#elif NET6_0 || NETSTANDARD2_1
        public uint Index { get; }
#endif

#if NET8_0_OR_GREATER
#elif NET6_0 || NETSTANDARD2_1
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="item">数据</param>
        /// <param name="index">索引</param>
        public TvForEachItem(T item, uint index)
        {
            Item = item;
            Index = index;
        }
#endif
    }
}