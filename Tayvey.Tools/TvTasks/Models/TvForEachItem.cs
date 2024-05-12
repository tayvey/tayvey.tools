namespace Tayvey.Tools.TvTasks.Models
{
    /// <summary>
    /// Tv异步遍历项
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public sealed class TvForEachItem<T>
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Item { get; }

        /// <summary>
        /// 索引
        /// </summary>
        public uint Index { get; }

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
    }
}